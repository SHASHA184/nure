"""
Практична робота №1 - Варіант 19
Розрахунок страхового ризику автомобіля
Приклад реалізації моделей прогнозування
"""

import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.linear_model import LogisticRegression
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report, roc_auc_score, roc_curve, confusion_matrix
import matplotlib.pyplot as plt
import seaborn as sns

# Налаштування для відображення
plt.rcParams['figure.figsize'] = (12, 6)
sns.set_style('whitegrid')


def generate_sample_data(n_samples=10000):
    """
    Генерація синтетичних даних для демонстрації
    В реальному проекті дані завантажуються з бази даних або CSV
    """
    np.random.seed(42)

    # Генерація ознак
    age = np.random.randint(18, 70, n_samples)
    years_of_experience = np.clip(age - 18 - np.random.randint(0, 5, n_samples), 0, None)
    gender = np.random.choice(['M', 'F'], n_samples)
    car_age = np.random.randint(0, 20, n_samples)
    car_value = np.random.gamma(20, 1000, n_samples)
    annual_mileage = np.random.gamma(10, 1000, n_samples)
    previous_claims = np.random.poisson(0.3, n_samples)
    violations = np.random.poisson(0.5, n_samples)
    region = np.random.choice(['urban', 'suburban', 'rural'], n_samples, p=[0.5, 0.3, 0.2])
    car_brand = np.random.choice(['Toyota', 'BMW', 'Mercedes', 'Volkswagen', 'Ford'],
                                  n_samples, p=[0.25, 0.2, 0.15, 0.25, 0.15])

    # Розрахунок ймовірності ДТП (цільова змінна)
    # Більший ризик для:
    # - молодих водіїв (age < 25)
    # - з малим досвідом (years_of_experience < 3)
    # - з попередніми claims
    # - з порушеннями

    risk_score = (
        (age < 25) * 2.0 +
        (years_of_experience < 3) * 1.5 +
        previous_claims * 1.5 +
        violations * 1.0 +
        (annual_mileage > 20000) * 0.5 +
        (car_age > 10) * 0.3 +
        (region == 'urban') * 0.5
    )

    # Конвертація в ймовірність
    probability = 1 / (1 + np.exp(-0.3 * (risk_score - 5)))
    claim_occurred = np.random.binomial(1, probability)

    # Розмір збитків (якщо сталася ДТП)
    claim_amount = np.where(
        claim_occurred == 1,
        np.random.gamma(5, 500, n_samples) * (1 + 0.1 * car_value / 1000),
        0
    )

    # Створення DataFrame
    df = pd.DataFrame({
        'age': age,
        'years_of_experience': years_of_experience,
        'gender': gender,
        'car_age': car_age,
        'car_value': car_value,
        'annual_mileage': annual_mileage,
        'previous_claims': previous_claims,
        'violations': violations,
        'region': region,
        'car_brand': car_brand,
        'claim_occurred': claim_occurred,
        'claim_amount': claim_amount
    })

    return df


def preprocess_data(df):
    """
    Попередня обробка даних
    """
    # Feature Engineering
    df['age_group'] = pd.cut(df['age'], bins=[18, 25, 35, 50, 70],
                              labels=['young', 'middle', 'mature', 'senior'])
    df['risk_score'] = (df['previous_claims'] * 10 +
                        df['violations'] * 5 -
                        df['years_of_experience'] * 2)
    df['car_value_category'] = pd.cut(df['car_value'],
                                       bins=[0, 10000, 25000, 50000, np.inf],
                                       labels=['budget', 'medium', 'premium', 'luxury'])

    # Кодування категоріальних змінних
    le_gender = LabelEncoder()
    df['gender_encoded'] = le_gender.fit_transform(df['gender'])

    # One-hot encoding
    df = pd.get_dummies(df, columns=['region', 'car_brand', 'age_group', 'car_value_category'],
                        drop_first=True)

    return df


def train_logistic_regression(X_train, X_test, y_train, y_test):
    """
    Навчання логістичної регресії
    """
    print("=" * 60)
    print("ЛОГІСТИЧНА РЕГРЕСІЯ (Baseline)")
    print("=" * 60)

    model = LogisticRegression(max_iter=1000, random_state=42, class_weight='balanced')
    model.fit(X_train, y_train)

    # Прогнози
    y_pred = model.predict(X_test)
    y_proba = model.predict_proba(X_test)[:, 1]

    # Метрики
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred))
    print(f"AUC-ROC: {roc_auc_score(y_test, y_proba):.4f}")

    # Cross-validation
    cv_scores = cross_val_score(model, X_train, y_train, cv=5, scoring='roc_auc')
    print(f"\nCross-validation AUC-ROC: {cv_scores.mean():.4f} (+/- {cv_scores.std() * 2:.4f})")

    # Confusion Matrix
    cm = confusion_matrix(y_test, y_pred)
    plt.figure(figsize=(8, 6))
    sns.heatmap(cm, annot=True, fmt='d', cmap='Blues')
    plt.title('Confusion Matrix - Logistic Regression')
    plt.ylabel('Actual')
    plt.xlabel('Predicted')
    plt.tight_layout()
    plt.savefig('practical1/docs/confusion_matrix_lr.png', dpi=300)
    print("\n✓ Confusion matrix збережено: confusion_matrix_lr.png")

    return model, y_proba


def train_random_forest(X_train, X_test, y_train, y_test, feature_names):
    """
    Навчання Random Forest
    """
    print("\n" + "=" * 60)
    print("RANDOM FOREST")
    print("=" * 60)

    model = RandomForestClassifier(
        n_estimators=200,
        max_depth=10,
        min_samples_split=20,
        min_samples_leaf=10,
        random_state=42,
        class_weight='balanced',
        n_jobs=-1
    )
    model.fit(X_train, y_train)

    # Прогнози
    y_pred = model.predict(X_test)
    y_proba = model.predict_proba(X_test)[:, 1]

    # Метрики
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred))
    print(f"AUC-ROC: {roc_auc_score(y_test, y_proba):.4f}")

    # Feature Importance
    feature_importance = pd.DataFrame({
        'feature': feature_names,
        'importance': model.feature_importances_
    }).sort_values('importance', ascending=False)

    print("\nTop 15 Important Features:")
    print(feature_importance.head(15).to_string(index=False))

    # Візуалізація важливості ознак
    plt.figure(figsize=(12, 8))
    top_features = feature_importance.head(15)
    plt.barh(range(len(top_features)), top_features['importance'])
    plt.yticks(range(len(top_features)), top_features['feature'])
    plt.xlabel('Importance')
    plt.title('Top 15 Feature Importances - Random Forest')
    plt.gca().invert_yaxis()
    plt.tight_layout()
    plt.savefig('practical1/docs/feature_importance.png', dpi=300)
    print("\n✓ Feature importance збережено: feature_importance.png")

    return model, y_proba


def plot_roc_curves(y_test, lr_proba, rf_proba):
    """
    Порівняння ROC кривих
    """
    plt.figure(figsize=(10, 8))

    # Logistic Regression
    fpr_lr, tpr_lr, _ = roc_curve(y_test, lr_proba)
    auc_lr = roc_auc_score(y_test, lr_proba)
    plt.plot(fpr_lr, tpr_lr, label=f'Logistic Regression (AUC = {auc_lr:.3f})', linewidth=2)

    # Random Forest
    fpr_rf, tpr_rf, _ = roc_curve(y_test, rf_proba)
    auc_rf = roc_auc_score(y_test, rf_proba)
    plt.plot(fpr_rf, tpr_rf, label=f'Random Forest (AUC = {auc_rf:.3f})', linewidth=2)

    # Baseline
    plt.plot([0, 1], [0, 1], 'k--', label='Random Classifier', linewidth=2)

    plt.xlabel('False Positive Rate', fontsize=12)
    plt.ylabel('True Positive Rate', fontsize=12)
    plt.title('ROC Curves Comparison', fontsize=14, fontweight='bold')
    plt.legend(fontsize=11, loc='lower right')
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.savefig('practical1/docs/roc_curves.png', dpi=300)
    print("\n✓ ROC curves збережено: roc_curves.png")


def calculate_insurance_premium(df, claim_probability):
    """
    Розрахунок страхової премії
    """
    print("\n" + "=" * 60)
    print("РОЗРАХУНОК СТРАХОВОЇ ПРЕМІЇ")
    print("=" * 60)

    # Середній розмір збитків для тих, хто мав ДТП
    claims_data = df[df['claim_occurred'] == 1]
    avg_claim_amount = claims_data['claim_amount'].mean()

    print(f"\nСередній розмір збитків при ДТП: ${avg_claim_amount:,.2f}")
    print(f"Медіана розміру збитків: ${claims_data['claim_amount'].median():,.2f}")

    # Розрахунок премії
    base_premium = 500  # Базова премія
    loading_factor = 1.35  # 35% навантаження

    expected_loss = claim_probability * avg_claim_amount
    insurance_premium = base_premium + expected_loss * loading_factor

    print(f"\nБазова премія: ${base_premium}")
    print(f"Навантаження: {(loading_factor - 1) * 100:.0f}%")
    print(f"\nСередня страхова премія: ${np.mean(insurance_premium):,.2f}")
    print(f"Мінімальна премія: ${np.min(insurance_premium):,.2f}")
    print(f"Максимальна премія: ${np.max(insurance_premium):,.2f}")
    print(f"Медіана премії: ${np.median(insurance_premium):,.2f}")

    # Розподіл премій
    plt.figure(figsize=(12, 5))

    plt.subplot(1, 2, 1)
    plt.hist(insurance_premium, bins=50, edgecolor='black', alpha=0.7)
    plt.xlabel('Insurance Premium ($)')
    plt.ylabel('Frequency')
    plt.title('Distribution of Insurance Premiums')
    plt.axvline(np.mean(insurance_premium), color='red', linestyle='--',
                label=f'Mean: ${np.mean(insurance_premium):,.0f}')
    plt.axvline(np.median(insurance_premium), color='green', linestyle='--',
                label=f'Median: ${np.median(insurance_premium):,.0f}')
    plt.legend()
    plt.grid(True, alpha=0.3)

    plt.subplot(1, 2, 2)
    plt.scatter(claim_probability, insurance_premium, alpha=0.3)
    plt.xlabel('Claim Probability')
    plt.ylabel('Insurance Premium ($)')
    plt.title('Premium vs. Risk')
    plt.grid(True, alpha=0.3)

    plt.tight_layout()
    plt.savefig('practical1/docs/insurance_premiums.png', dpi=300)
    print("\n✓ Premium distribution збережено: insurance_premiums.png")

    return insurance_premium


def main():
    """
    Головна функція
    """
    print("\n" + "=" * 60)
    print("ПРАКТИЧНА РОБОТА №1 - ВАРІАНТ 19")
    print("Розрахунок страхового ризику автомобіля")
    print("=" * 60)

    # 1. Генерація даних
    print("\n[1/6] Генерація даних...")
    df = generate_sample_data(n_samples=15000)
    print(f"✓ Згенеровано {len(df)} записів")
    print(f"  Частка ДТП: {df['claim_occurred'].mean() * 100:.2f}%")

    # 2. Попередня обробка
    print("\n[2/6] Попередня обробка даних...")
    df = preprocess_data(df)
    print(f"✓ Створено {df.shape[1]} ознак")

    # Підготовка даних для моделювання
    feature_cols = [col for col in df.columns
                    if col not in ['claim_occurred', 'claim_amount', 'gender']]
    X = df[feature_cols]
    y = df['claim_occurred']

    # Train/Test split
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )

    # Стандартизація числових ознак
    numerical_cols = ['age', 'years_of_experience', 'car_age', 'car_value',
                      'annual_mileage', 'previous_claims', 'violations', 'risk_score']
    scaler = StandardScaler()
    X_train[numerical_cols] = scaler.fit_transform(X_train[numerical_cols])
    X_test[numerical_cols] = scaler.transform(X_test[numerical_cols])

    print(f"✓ Train set: {len(X_train)} зразків")
    print(f"✓ Test set: {len(X_test)} зразків")

    # 3. Навчання Logistic Regression
    print("\n[3/6] Навчання Logistic Regression...")
    lr_model, lr_proba = train_logistic_regression(X_train, X_test, y_train, y_test)

    # 4. Навчання Random Forest
    print("\n[4/6] Навчання Random Forest...")
    rf_model, rf_proba = train_random_forest(X_train, X_test, y_train, y_test,
                                              feature_cols)

    # 5. Порівняння моделей
    print("\n[5/6] Порівняння моделей...")
    plot_roc_curves(y_test, lr_proba, rf_proba)

    # 6. Розрахунок страхової премії
    print("\n[6/6] Розрахунок страхової премії...")
    insurance_premium = calculate_insurance_premium(df, rf_proba)

    print("\n" + "=" * 60)
    print("ЗАВЕРШЕНО!")
    print("=" * 60)
    print("\nГрафіки збережено в директорії practical1/docs/:")
    print("  - confusion_matrix_lr.png")
    print("  - feature_importance.png")
    print("  - roc_curves.png")
    print("  - insurance_premiums.png")
    print("\nДетальний звіт: practical1/docs/variant19_report.md")
    print("=" * 60 + "\n")


if __name__ == "__main__":
    main()
