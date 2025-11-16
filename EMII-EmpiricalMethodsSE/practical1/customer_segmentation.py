"""
Практична робота №1 - Варіант 19
Сегментація клієнтів за ризиком
Демонстрація кластеризації (K-Means, DBSCAN)
"""

import pandas as pd
import numpy as np
from sklearn.cluster import KMeans, DBSCAN
from sklearn.preprocessing import StandardScaler
from sklearn.decomposition import PCA
from sklearn.metrics import silhouette_score, davies_bouldin_score
import matplotlib.pyplot as plt
import seaborn as sns

# Налаштування
plt.rcParams['figure.figsize'] = (14, 8)
sns.set_style('whitegrid')
sns.set_palette('husl')


def generate_customer_data(n_samples=5000):
    """
    Генерація даних клієнтів для сегментації
    """
    np.random.seed(42)

    # Створюємо 5 природних кластерів з різними характеристиками

    # Кластер 1: Низький ризик - молоді професіонали
    n1 = n_samples // 5
    cluster1 = pd.DataFrame({
        'age': np.random.normal(30, 3, n1),
        'years_of_experience': np.random.normal(8, 2, n1),
        'annual_mileage': np.random.normal(12000, 2000, n1),
        'car_value': np.random.normal(25000, 5000, n1),
        'previous_claims': np.random.poisson(0.1, n1),
        'violations': np.random.poisson(0.2, n1),
        'true_cluster': 0
    })

    # Кластер 2: Помірний ризик - середній вік
    n2 = n_samples // 5
    cluster2 = pd.DataFrame({
        'age': np.random.normal(45, 5, n2),
        'years_of_experience': np.random.normal(20, 5, n2),
        'annual_mileage': np.random.normal(15000, 3000, n2),
        'car_value': np.random.normal(20000, 4000, n2),
        'previous_claims': np.random.poisson(0.5, n2),
        'violations': np.random.poisson(0.5, n2),
        'true_cluster': 1
    })

    # Кластер 3: Високий ризик - молоді водії
    n3 = n_samples // 5
    cluster3 = pd.DataFrame({
        'age': np.random.normal(22, 2, n3),
        'years_of_experience': np.random.normal(2, 1, n3),
        'annual_mileage': np.random.normal(18000, 4000, n3),
        'car_value': np.random.normal(30000, 6000, n3),
        'previous_claims': np.random.poisson(1.2, n3),
        'violations': np.random.poisson(1.5, n3),
        'true_cluster': 2
    })

    # Кластер 4: Premium - дорогі авто, досвідчені
    n4 = n_samples // 5
    cluster4 = pd.DataFrame({
        'age': np.random.normal(50, 7, n4),
        'years_of_experience': np.random.normal(25, 5, n4),
        'annual_mileage': np.random.normal(10000, 2000, n4),
        'car_value': np.random.normal(60000, 15000, n4),
        'previous_claims': np.random.poisson(0.3, n4),
        'violations': np.random.poisson(0.1, n4),
        'true_cluster': 3
    })

    # Кластер 5: Група ризику - багато порушень
    n5 = n_samples - n1 - n2 - n3 - n4
    cluster5 = pd.DataFrame({
        'age': np.random.normal(35, 8, n5),
        'years_of_experience': np.random.normal(12, 6, n5),
        'annual_mileage': np.random.normal(25000, 5000, n5),
        'car_value': np.random.normal(18000, 4000, n5),
        'previous_claims': np.random.poisson(2.5, n5),
        'violations': np.random.poisson(3.0, n5),
        'true_cluster': 4
    })

    # Об'єднання всіх кластерів
    df = pd.concat([cluster1, cluster2, cluster3, cluster4, cluster5], ignore_index=True)

    # Обрізання від'ємних значень
    df = df.clip(lower=0)

    # Додавання ID клієнта
    df.insert(0, 'customer_id', range(1, len(df) + 1))

    return df


def explore_data(df):
    """
    Дослідницький аналіз даних
    """
    print("=" * 60)
    print("ДОСЛІДНИЦЬКИЙ АНАЛІЗ ДАНИХ")
    print("=" * 60)

    print("\nРозмір датасету:", df.shape)
    print("\nПерші 5 записів:")
    print(df.head())

    print("\nОписова статистика:")
    print(df.describe())

    print("\nПеревірка на пропущені значення:")
    print(df.isnull().sum())

    # Кореляційна матриця
    feature_cols = ['age', 'years_of_experience', 'annual_mileage',
                    'car_value', 'previous_claims', 'violations']

    plt.figure(figsize=(10, 8))
    correlation_matrix = df[feature_cols].corr()
    sns.heatmap(correlation_matrix, annot=True, fmt='.2f', cmap='coolwarm',
                center=0, square=True, linewidths=1)
    plt.title('Correlation Matrix', fontsize=14, fontweight='bold')
    plt.tight_layout()
    plt.savefig('practical1/docs/correlation_matrix.png', dpi=300)
    print("\n✓ Кореляційну матрицю збережено: correlation_matrix.png")

    # Розподіл ознак
    fig, axes = plt.subplots(2, 3, figsize=(16, 10))
    axes = axes.ravel()

    for idx, col in enumerate(feature_cols):
        axes[idx].hist(df[col], bins=50, edgecolor='black', alpha=0.7)
        axes[idx].set_title(col, fontweight='bold')
        axes[idx].set_xlabel('Value')
        axes[idx].set_ylabel('Frequency')
        axes[idx].grid(True, alpha=0.3)

    plt.tight_layout()
    plt.savefig('practical1/docs/feature_distributions.png', dpi=300)
    print("✓ Розподіл ознак збережено: feature_distributions.png")


def find_optimal_k(X_scaled, k_range=range(2, 11)):
    """
    Визначення оптимальної кількості кластерів
    """
    print("\n" + "=" * 60)
    print("ВИЗНАЧЕННЯ ОПТИМАЛЬНОЇ КІЛЬКОСТІ КЛАСТЕРІВ")
    print("=" * 60)

    inertias = []
    silhouette_scores = []
    davies_bouldin_scores = []

    for k in k_range:
        kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
        labels = kmeans.fit_predict(X_scaled)

        inertias.append(kmeans.inertia_)
        silhouette_scores.append(silhouette_score(X_scaled, labels))
        davies_bouldin_scores.append(davies_bouldin_score(X_scaled, labels))

        print(f"K={k}: Inertia={kmeans.inertia_:.0f}, "
              f"Silhouette={silhouette_score(X_scaled, labels):.4f}, "
              f"Davies-Bouldin={davies_bouldin_score(X_scaled, labels):.4f}")

    # Візуалізація
    fig, axes = plt.subplots(1, 3, figsize=(18, 5))

    # Elbow method
    axes[0].plot(k_range, inertias, 'bo-', linewidth=2, markersize=8)
    axes[0].set_xlabel('Number of Clusters (K)', fontsize=12)
    axes[0].set_ylabel('Inertia', fontsize=12)
    axes[0].set_title('Elbow Method', fontsize=14, fontweight='bold')
    axes[0].grid(True, alpha=0.3)

    # Silhouette Score (вище = краще)
    axes[1].plot(k_range, silhouette_scores, 'ro-', linewidth=2, markersize=8)
    axes[1].set_xlabel('Number of Clusters (K)', fontsize=12)
    axes[1].set_ylabel('Silhouette Score', fontsize=12)
    axes[1].set_title('Silhouette Analysis', fontsize=14, fontweight='bold')
    axes[1].grid(True, alpha=0.3)

    # Davies-Bouldin Index (нижче = краще)
    axes[2].plot(k_range, davies_bouldin_scores, 'go-', linewidth=2, markersize=8)
    axes[2].set_xlabel('Number of Clusters (K)', fontsize=12)
    axes[2].set_ylabel('Davies-Bouldin Index', fontsize=12)
    axes[2].set_title('Davies-Bouldin Index', fontsize=14, fontweight='bold')
    axes[2].grid(True, alpha=0.3)

    plt.tight_layout()
    plt.savefig('practical1/docs/cluster_optimization.png', dpi=300)
    print("\n✓ Графіки оптимізації збережено: cluster_optimization.png")

    # Оптимальний K за Silhouette Score
    optimal_k = k_range[np.argmax(silhouette_scores)]
    print(f"\n✓ Рекомендований K (за Silhouette Score): {optimal_k}")

    return optimal_k


def perform_kmeans(df, X_scaled, k):
    """
    Виконання K-Means кластеризації
    """
    print("\n" + "=" * 60)
    print(f"K-MEANS КЛАСТЕРИЗАЦІЯ (K={k})")
    print("=" * 60)

    kmeans = KMeans(n_clusters=k, random_state=42, n_init=10, max_iter=300)
    clusters = kmeans.fit_predict(X_scaled)

    # Додавання кластерів до DataFrame
    df['cluster'] = clusters

    # Аналіз кластерів
    feature_cols = ['age', 'years_of_experience', 'annual_mileage',
                    'car_value', 'previous_claims', 'violations']

    print("\nСтатистика по кластерам:")
    cluster_summary = df.groupby('cluster')[feature_cols].agg(['mean', 'std', 'count'])
    print(cluster_summary)

    # Візуалізація розмірів кластерів
    cluster_sizes = df['cluster'].value_counts().sort_index()
    plt.figure(figsize=(10, 6))
    plt.bar(cluster_sizes.index, cluster_sizes.values, edgecolor='black', alpha=0.7)
    plt.xlabel('Cluster ID', fontsize=12)
    plt.ylabel('Number of Customers', fontsize=12)
    plt.title('Cluster Sizes', fontsize=14, fontweight='bold')
    plt.grid(True, alpha=0.3, axis='y')
    for i, v in enumerate(cluster_sizes.values):
        plt.text(i, v + 50, str(v), ha='center', fontweight='bold')
    plt.tight_layout()
    plt.savefig('practical1/docs/cluster_sizes.png', dpi=300)
    print("\n✓ Розміри кластерів збережено: cluster_sizes.png")

    # Інтерпретація кластерів
    interpret_clusters(df, feature_cols)

    return kmeans, clusters


def interpret_clusters(df, feature_cols):
    """
    Інтерпретація та найменування кластерів
    """
    print("\n" + "=" * 60)
    print("ІНТЕРПРЕТАЦІЯ КЛАСТЕРІВ")
    print("=" * 60)

    cluster_means = df.groupby('cluster')[feature_cols].mean()

    cluster_names = {}
    for cluster_id in sorted(df['cluster'].unique()):
        profile = cluster_means.loc[cluster_id]

        # Логіка найменування на основі характеристик
        if profile['age'] < 25 and profile['years_of_experience'] < 5:
            name = 'High Risk - Young Inexperienced'
        elif profile['car_value'] > 50000:
            name = 'Premium - Luxury Cars'
        elif profile['previous_claims'] > 2 or profile['violations'] > 2:
            name = 'Risk Group - Many Claims/Violations'
        elif profile['age'] > 45 and profile['years_of_experience'] > 20:
            name = 'Low Risk - Mature Experienced'
        else:
            name = 'Moderate Risk - Average'

        cluster_names[cluster_id] = name

        print(f"\nКластер {cluster_id}: {name}")
        print(f"  Середній вік: {profile['age']:.1f} років")
        print(f"  Досвід водіння: {profile['years_of_experience']:.1f} років")
        print(f"  Вартість авто: ${profile['car_value']:,.0f}")
        print(f"  Річний пробіг: {profile['annual_mileage']:,.0f} км")
        print(f"  Попередні claims: {profile['previous_claims']:.2f}")
        print(f"  Порушення: {profile['violations']:.2f}")
        print(f"  Розмір кластеру: {len(df[df['cluster'] == cluster_id])} клієнтів")

    df['cluster_name'] = df['cluster'].map(cluster_names)

    # Збереження інтерпретації
    cluster_interpretation = pd.DataFrame({
        'Cluster ID': cluster_names.keys(),
        'Cluster Name': cluster_names.values()
    })
    cluster_interpretation.to_csv('practical1/docs/cluster_names.csv', index=False)
    print("\n✓ Інтерпретацію кластерів збережено: cluster_names.csv")


def visualize_clusters_pca(X_scaled, clusters, cluster_centers):
    """
    Візуалізація кластерів за допомогою PCA
    """
    print("\n" + "=" * 60)
    print("ВІЗУАЛІЗАЦІЯ КЛАСТЕРІВ (PCA)")
    print("=" * 60)

    # PCA для зменшення до 2D
    pca = PCA(n_components=2, random_state=42)
    X_pca = pca.fit_transform(X_scaled)
    centers_pca = pca.transform(cluster_centers)

    print(f"\nPCA explained variance:")
    print(f"  PC1: {pca.explained_variance_ratio_[0] * 100:.2f}%")
    print(f"  PC2: {pca.explained_variance_ratio_[1] * 100:.2f}%")
    print(f"  Total: {pca.explained_variance_ratio_.sum() * 100:.2f}%")

    # Візуалізація
    plt.figure(figsize=(14, 10))

    scatter = plt.scatter(X_pca[:, 0], X_pca[:, 1], c=clusters, cmap='viridis',
                          alpha=0.6, edgecolors='k', s=50)

    # Центроїди
    plt.scatter(centers_pca[:, 0], centers_pca[:, 1],
                c='red', marker='X', s=300, edgecolors='black',
                linewidths=2, label='Centroids')

    # Підписи центроїдів
    for i, center in enumerate(centers_pca):
        plt.annotate(f'C{i}', (center[0], center[1]),
                     fontsize=14, fontweight='bold',
                     bbox=dict(boxstyle='round,pad=0.3', facecolor='yellow', alpha=0.7))

    plt.xlabel(f'PC1 ({pca.explained_variance_ratio_[0] * 100:.1f}% variance)',
               fontsize=12)
    plt.ylabel(f'PC2 ({pca.explained_variance_ratio_[1] * 100:.1f}% variance)',
               fontsize=12)
    plt.title('Customer Segmentation - K-Means Clustering (PCA Projection)',
              fontsize=14, fontweight='bold')
    plt.colorbar(scatter, label='Cluster ID')
    plt.legend(fontsize=11)
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.savefig('practical1/docs/clusters_pca.png', dpi=300)
    print("\n✓ PCA візуалізацію збережено: clusters_pca.png")


def perform_dbscan(X_scaled):
    """
    Виконання DBSCAN кластеризації
    """
    print("\n" + "=" * 60)
    print("DBSCAN КЛАСТЕРИЗАЦІЯ")
    print("=" * 60)

    # Підбір параметрів eps та min_samples
    dbscan = DBSCAN(eps=0.8, min_samples=10)
    clusters = dbscan.fit_predict(X_scaled)

    n_clusters = len(set(clusters)) - (1 if -1 in clusters else 0)
    n_noise = list(clusters).count(-1)

    print(f"\nКількість кластерів: {n_clusters}")
    print(f"Кількість outliers (noise points): {n_noise}")
    print(f"Відсоток outliers: {n_noise / len(clusters) * 100:.2f}%")

    # Розподіл по кластерам
    unique, counts = np.unique(clusters, return_counts=True)
    print("\nРозподіл точок по кластерам:")
    for cluster_id, count in zip(unique, counts):
        if cluster_id == -1:
            print(f"  Noise: {count} точок")
        else:
            print(f"  Cluster {cluster_id}: {count} точок")

    # Візуалізація
    pca = PCA(n_components=2, random_state=42)
    X_pca = pca.fit_transform(X_scaled)

    plt.figure(figsize=(14, 10))

    # Кластери
    unique_clusters = set(clusters)
    colors = plt.cm.Spectral(np.linspace(0, 1, len(unique_clusters)))

    for cluster_id, color in zip(unique_clusters, colors):
        if cluster_id == -1:
            # Noise points - чорні хрестики
            mask = clusters == cluster_id
            plt.scatter(X_pca[mask, 0], X_pca[mask, 1],
                        c='black', marker='x', s=50, alpha=0.5, label='Noise')
        else:
            mask = clusters == cluster_id
            plt.scatter(X_pca[mask, 0], X_pca[mask, 1],
                        c=[color], marker='o', s=50, alpha=0.7,
                        edgecolors='k', label=f'Cluster {cluster_id}')

    plt.xlabel(f'PC1 ({pca.explained_variance_ratio_[0] * 100:.1f}% variance)',
               fontsize=12)
    plt.ylabel(f'PC2 ({pca.explained_variance_ratio_[1] * 100:.1f}% variance)',
               fontsize=12)
    plt.title('DBSCAN Clustering (PCA Projection)', fontsize=14, fontweight='bold')
    plt.legend(fontsize=10, loc='best')
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.savefig('practical1/docs/dbscan_clusters.png', dpi=300)
    print("\n✓ DBSCAN візуалізацію збережено: dbscan_clusters.png")

    return clusters


def visualize_cluster_profiles(df):
    """
    Радарна діаграма профілів кластерів
    """
    print("\n" + "=" * 60)
    print("ПРОФІЛІ КЛАСТЕРІВ (RADAR CHART)")
    print("=" * 60)

    feature_cols = ['age', 'years_of_experience', 'annual_mileage',
                    'car_value', 'previous_claims', 'violations']

    # Нормалізація для радарної діаграми (0-1)
    from sklearn.preprocessing import MinMaxScaler
    scaler = MinMaxScaler()
    df_normalized = df.copy()
    df_normalized[feature_cols] = scaler.fit_transform(df[feature_cols])

    cluster_profiles = df_normalized.groupby('cluster')[feature_cols].mean()

    # Радарна діаграма
    from math import pi

    num_vars = len(feature_cols)
    angles = [n / float(num_vars) * 2 * pi for n in range(num_vars)]
    angles += angles[:1]

    fig, ax = plt.subplots(figsize=(12, 12), subplot_kw=dict(projection='polar'))

    for idx, cluster_id in enumerate(cluster_profiles.index):
        values = cluster_profiles.loc[cluster_id].tolist()
        values += values[:1]

        ax.plot(angles, values, 'o-', linewidth=2, label=f'Cluster {cluster_id}')
        ax.fill(angles, values, alpha=0.15)

    ax.set_xticks(angles[:-1])
    ax.set_xticklabels(feature_cols, size=11)
    ax.set_ylim(0, 1)
    ax.set_title('Cluster Profiles (Normalized)', size=16, fontweight='bold', pad=20)
    ax.legend(loc='upper right', bbox_to_anchor=(1.3, 1.1))
    ax.grid(True)

    plt.tight_layout()
    plt.savefig('practical1/docs/cluster_radar.png', dpi=300, bbox_inches='tight')
    print("\n✓ Радарну діаграму збережено: cluster_radar.png")


def main():
    """
    Головна функція
    """
    print("\n" + "=" * 60)
    print("ПРАКТИЧНА РОБОТА №1 - ВАРІАНТ 19")
    print("Сегментація клієнтів за ризиком")
    print("=" * 60)

    # 1. Генерація даних
    print("\n[1/7] Генерація даних клієнтів...")
    df = generate_customer_data(n_samples=5000)
    print(f"✓ Згенеровано {len(df)} профілів клієнтів")

    # 2. Дослідницький аналіз
    print("\n[2/7] Дослідницький аналіз даних...")
    explore_data(df)

    # 3. Підготовка даних
    print("\n[3/7] Підготовка даних для кластеризації...")
    feature_cols = ['age', 'years_of_experience', 'annual_mileage',
                    'car_value', 'previous_claims', 'violations']
    X = df[feature_cols]

    # Стандартизація (критично важливо!)
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    print(f"✓ Дані стандартизовано: {X_scaled.shape}")

    # 4. Визначення оптимального K
    print("\n[4/7] Визначення оптимальної кількості кластерів...")
    optimal_k = find_optimal_k(X_scaled, k_range=range(2, 11))

    # 5. K-Means кластеризація
    print("\n[5/7] Виконання K-Means кластеризації...")
    kmeans_model, clusters = perform_kmeans(df, X_scaled, k=optimal_k)

    # 6. Візуалізація
    print("\n[6/7] Візуалізація результатів...")
    visualize_clusters_pca(X_scaled, clusters, kmeans_model.cluster_centers_)
    visualize_cluster_profiles(df)

    # 7. DBSCAN (додатково)
    print("\n[7/7] DBSCAN кластеризація (виявлення outliers)...")
    dbscan_clusters = perform_dbscan(X_scaled)

    # Збереження результатів
    df[['customer_id', 'cluster', 'cluster_name']].to_csv(
        'practical1/docs/customer_segments.csv', index=False
    )
    print("\n✓ Сегменти клієнтів збережено: customer_segments.csv")

    print("\n" + "=" * 60)
    print("ЗАВЕРШЕНО!")
    print("=" * 60)
    print("\nГрафіки збережено в директорії practical1/docs/:")
    print("  - correlation_matrix.png")
    print("  - feature_distributions.png")
    print("  - cluster_optimization.png")
    print("  - cluster_sizes.png")
    print("  - clusters_pca.png")
    print("  - cluster_radar.png")
    print("  - dbscan_clusters.png")
    print("\nДані збережено:")
    print("  - cluster_names.csv")
    print("  - customer_segments.csv")
    print("=" * 60 + "\n")


if __name__ == "__main__":
    main()
