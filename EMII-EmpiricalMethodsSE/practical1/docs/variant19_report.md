# Практична робота №1
## Використання емпіричних моделей та методів у розробці програмного забезпечення

**Варіант 19: Розрахунок страхового ризику автомобіля**

---

## 1. Постановка задачі

Необхідно дослідити та описати використання статистичних методів та емпіричних моделей у процесі розробки програмного забезпечення для розрахунку страхового ризику автомобіля. Визначити основні ситуації, що потребують статистичного аналізу, та описати відповідні методи й інструменти.

---

## 2. Опис предметної області

Розрахунок страхового ризику автомобіля - це процес оцінки ймовірності настання страхового випадку та потенційного розміру збитків для визначення справедливої страхової премії. Це критично важлива область для страхових компаній, яка безпосередньо впливає на їх прибутковість та конкурентоспроможність.

### Основні завдання системи:
- Прогнозування ймовірності ДТП для конкретного водія/автомобіля
- Розрахунок очікуваного розміру збитків
- Визначення оптимальної страхової премії
- Сегментація клієнтів за рівнем ризику
- Виявлення шахрайства (fraud detection)

---

## 3. Ситуації, що потребують статистичного аналізу

### Ситуація 1: Прогнозування ймовірності ДТП

**Опис проблеми:**
Необхідно створити модель, яка на основі характеристик водія (вік, стаж, історія порушень) та автомобіля (марка, модель, рік випуску, потужність) прогнозує ймовірність настання ДТП протягом наступного року.

**Вхідні дані:**
- Демографічні дані водія: вік, стать, місце проживання
- Досвід водіння: стаж, кількість попередніх страхових випадків
- Характеристики автомобіля: марка, модель, рік випуску, об'єм двигуна, потужність
- Історичні дані про ДТП за останні 5-10 років

**Статистичні методи:**
- **Логістична регресія** - для бінарної класифікації (ДТП/без ДТП)
- **Випадковий ліс (Random Forest)** - для врахування нелінійних залежностей
- **Градієнтний бустинг (XGBoost, LightGBM)** - для підвищення точності прогнозування
- **ROC-аналіз** - для оцінки якості моделі

**Очікуваний результат:**
Ймовірність ДТП для кожного клієнта з точністю 85-90% (AUC-ROC > 0.85)

---

### Ситуація 2: Розрахунок страхової премії

**Опис проблеми:**
На основі спрогнозованого ризику необхідно розрахувати справедливу страхову премію, яка покриває очікувані збитки та забезпечує прибуток страхової компанії.

**Вхідні дані:**
- Прогнозована ймовірність ДТП (з Ситуації 1)
- Історичні дані про розміри виплат
- Середня вартість ремонту для різних типів автомобілів
- Тяжкість пошкоджень (minor, moderate, severe, total loss)
- Економічні показники (інфляція, курс валют)

**Статистичні методи:**
- **Узагальнені лінійні моделі (GLM)** - зокрема Gamma або Tweedie розподіли для моделювання розміру збитків
- **Множинна лінійна регресія** - для базових оцінок
- **Квантильна регресія** - для оцінки різних сценаріїв (оптимістичний, базовий, песимістичний)
- **Метод Монте-Карло** - для симуляції різних сценаріїв збитків

**Очікуваний результат:**
Розрахунок страхової премії з врахуванням:
- Чистої премії (покриття очікуваних збитків)
- Навантаження на витрати (15-20%)
- Прибутку компанії (5-10%)
- Буфера на непередбачені випадки (10-15%)

---

### Ситуація 3: Сегментація клієнтів за ризиком

**Опис проблеми:**
Розподілити клієнтів на однорідні групи за рівнем ризику для диференційованого підходу до ціноутворення та маркетингових стратегій.

**Вхідні дані:**
- Профілі водіїв: всі доступні характеристики
- Історія взаємодії з компанією
- Поведінкові дані (якщо доступна телематика)
- Соціально-економічні показники регіону

**Статистичні методи:**
- **K-Means кластеризація** - для виділення основних сегментів
- **Ієрархічна кластеризація** - для визначення оптимальної кількості кластерів
- **DBSCAN** - для виявлення аномальних груп
- **Дискримінантний аналіз** - для валідації отриманих сегментів
- **Аналіз головних компонент (PCA)** - для зменшення розмірності даних

**Очікуваний результат:**
Виділення 5-7 чітких сегментів клієнтів:
- Низький ризик (молоді професіонали з новими авто)
- Помірний ризик (середній вік, середній досвід)
- Високий ризик (молоді водії, потужні авто)
- Premium сегмент (дорогі авто, досвідчені водії)
- Корпоративні клієнти
- Група ризику (багато порушень/ДТП)

---

## 4. Детальна таблиця опису ситуацій

| № | Ситуація | Стадія розробки ЖЦ ПЗ | ПЗ/Бібліотеки | Джерела даних | Формат даних | Обсяг вибірки |
|---|----------|----------------------|---------------|---------------|--------------|---------------|
| 1 | Прогнозування ймовірності ДТП | Проектування та реалізація моделі | **Python:** scikit-learn (LogisticRegression, RandomForestClassifier), XGBoost, LightGBM, pandas, numpy<br>**R:** caret, randomForest, glm<br>**Tableau/Power BI** для візуалізації | - Історичні дані страхової компанії<br>- Відкриті датасети ДТП (NHTSA, EuroRAP)<br>- Дані реєстру МВС<br>- Дані телематичних систем | CSV, JSON, Parquet, SQL бази даних (PostgreSQL, MySQL), API endpoints | Мінімум: 10,000 записів<br>Оптимально: 100,000+ записів<br>Період: 3-5 років |
| 2 | Розрахунок страхової премії | Тестування та валідація моделі | **Python:** statsmodels (GLM), scipy (statistics), numpy<br>**R:** MASS, glmnet, actuarial packages<br>**Excel** з надбудовами<br>**SAS** для актуарних розрахунків | - Актуарні таблиці<br>- Історія виплат компанії<br>- Дані про вартість ремонтів<br>- Статистика автосалонів<br>- Економічні індекси | CSV, XLSX, SQL бази даних, XML (для обміну з іншими системами) | 50,000+ історичних виплат<br>Період: 5-10 років<br>Актуарні таблиці: постійно оновлювані |
| 3 | Сегментація клієнтів за ризиком | Аналіз вимог та дослідження | **Python:** scikit-learn (KMeans, DBSCAN, AgglomerativeClustering), scipy.cluster<br>**R:** cluster, factoextra<br>**SPSS** для статистичного аналізу | - Повна база клієнтів компанії<br>- Профілі водіїв<br>- Історія страхових випадків<br>- CRM система<br>- Зовнішні датасети (демографія, економіка регіонів) | CSV, JSON, SQL бази даних, NoSQL (MongoDB) для профілів користувачів | 20,000+ клієнтів для надійної сегментації<br>З історією мінімум 1 рік |

---

## 5. Опис статистичних методів у ПЗ/бібліотеках

### 5.1. Логістична регресія (scikit-learn.LogisticRegression)

**Математична основа:**
```
P(Y=1|X) = 1 / (1 + e^(-(β₀ + β₁X₁ + β₂X₂ + ... + βₙXₙ)))
```

**Застосування:**
Прогнозування бінарного результату (ДТП/без ДТП). Модель оцінює ймовірність настання події на основі набору предикторів.

**Переваги:**
- Інтерпретованість коефіцієнтів
- Швидкість навчання
- Не вимагає великих обчислювальних ресурсів
- Добре працює при лінійній залежності

**Параметри в scikit-learn:**
- `penalty`: тип регуляризації (L1, L2, ElasticNet)
- `C`: зворотна сила регуляризації
- `solver`: алгоритм оптимізації (lbfgs, newton-cg, sag)
- `max_iter`: максимальна кількість ітерацій

**Метрики якості:**
- Accuracy, Precision, Recall, F1-Score
- AUC-ROC (площа під ROC-кривою)
- Confusion Matrix

---

### 5.2. Випадковий ліс (RandomForestClassifier/Regressor)

**Математична основа:**
Ансамбль дерев рішень, де кожне дерево навчається на випадковій підвибірці даних (bootstrap) та випадковій підмножині ознак.

```
Prediction = (1/N) * Σ(Tree_i predictions)
```

**Застосування:**
- Класифікація ризику (низький/середній/високий)
- Регресія для прогнозування розміру збитків
- Визначення важливості факторів (feature importance)

**Переваги:**
- Висока точність прогнозування
- Робастність до викидів та шуму
- Автоматичне визначення важливості ознак
- Не схильний до перенавчання (при достатній кількості дерев)

**Параметри:**
- `n_estimators`: кількість дерев (100-500)
- `max_depth`: максимальна глибина дерева
- `min_samples_split`: мінімальна кількість зразків для розділення вузла
- `max_features`: кількість ознак для розгляду при розділенні

**Інтерпретація результатів:**
- Feature importance scores - показують вклад кожної ознаки в прогноз
- Partial dependence plots - залежність прогнозу від однієї змінної

---

### 5.3. Градієнтний бустинг (XGBoost, LightGBM)

**Математична основа:**
Послідовне навчання слабких моделей (decision trees), де кожна наступна модель виправляє помилки попередніх.

```
F_m(x) = F_(m-1)(x) + γ_m * h_m(x)
```
де h_m(x) - нове дерево, що мінімізує функцію втрат.

**Застосування:**
Найточніші прогнози для складних нелінійних залежностей у страховому ризику.

**Переваги XGBoost:**
- State-of-the-art точність
- Вбудована регуляризація
- Обробка пропущених значень
- Паралелізація обчислень
- Ефективна робота з великими датасетами

**Основні гіперпараметри:**
- `learning_rate` (η): швидкість навчання (0.01-0.3)
- `max_depth`: глибина дерев (3-10)
- `n_estimators`: кількість дерев
- `subsample`: частка зразків для навчання кожного дерева
- `colsample_bytree`: частка ознак для кожного дерева
- `gamma`: мінімальне зменшення втрат для розділення
- `reg_alpha`, `reg_lambda`: L1 та L2 регуляризація

**Метрики:**
- RMSE (Root Mean Squared Error)
- MAE (Mean Absolute Error)
- AUC для класифікації

---

### 5.4. Узагальнені лінійні моделі - GLM (statsmodels)

**Математична основа:**
Розширення лінійної регресії для різних розподілів цільової змінної:

```
g(μ) = β₀ + β₁X₁ + ... + βₙXₙ
```
де g() - link function, μ = E(Y|X)

**Розподіли для страхування:**
- **Poisson** - для частоти страхових випадків (лічильні дані)
- **Gamma** - для розміру збитків (позитивні неперервні дані зі скосом)
- **Tweedie** - комбіноване моделювання частоти та серйозності

**Застосування:**
Актуарні розрахунки страхових премій. GLM - стандарт у страховій індустрії.

**Переваги:**
- Математична обґрунтованість
- Відповідність регуляторним вимогам
- Інтерпретованість для актуаріїв
- Гнучкість у виборі розподілу

**Приклад коду (Python):**
```python
import statsmodels.api as sm
from statsmodels.genmod.families import Gamma, Tweedie

# GLM для розміру збитків (Gamma розподіл)
gamma_model = sm.GLM(y_claims, X_features,
                     family=Gamma(link=sm.families.links.log()))
results = gamma_model.fit()
```

---

### 5.5. Кластеризація K-Means (scikit-learn)

**Математична основа:**
Розділення даних на K кластерів шляхом мінімізації внутрішньокластерної дисперсії:

```
arg min Σ Σ ||x_i - μ_j||²
```
де μ_j - центроїд кластеру j

**Алгоритм:**
1. Випадкова ініціалізація K центроїдів
2. Призначення кожної точки до найближчого центроїду
3. Перерахунок центроїдів як середніх точок кластерів
4. Повтор кроків 2-3 до збіжності

**Застосування:**
Сегментація клієнтів на групи з подібним профілем ризику.

**Вибір оптимального K:**
- **Elbow method** - графік залежності інерції від кількості кластерів
- **Silhouette score** - міра якості кластеризації (-1 до 1)
- **Business logic** - скільки сегментів має сенс для бізнесу

**Приклад коду:**
```python
from sklearn.cluster import KMeans
from sklearn.preprocessing import StandardScaler

# Стандартизація даних (важливо для K-Means!)
scaler = StandardScaler()
X_scaled = scaler.fit_transform(X_features)

# Кластеризація
kmeans = KMeans(n_clusters=6, random_state=42, n_init=10)
clusters = kmeans.fit_predict(X_scaled)

# Оцінка якості
from sklearn.metrics import silhouette_score
score = silhouette_score(X_scaled, clusters)
print(f"Silhouette Score: {score}")
```

---

### 5.6. DBSCAN (Density-Based Spatial Clustering)

**Математична основа:**
Кластеризація на основі щільності точок. Точка є core point, якщо в радіусі ε є мінімум minPts точок.

**Параметри:**
- `eps` (ε): радіус околиці
- `min_samples`: мінімальна кількість точок для формування кластеру

**Переваги:**
- Не потребує заздалегідь визначеної кількості кластерів
- Виявляє кластери довільної форми
- Ідентифікує викиди (outliers) як noise points

**Застосування в страхуванні:**
- Виявлення аномальних груп клієнтів (потенційне шахрайство)
- Ідентифікація унікальних профілів ризику

---

## 6. Формулювання гіпотез

### Гіпотеза 1: Точність прогнозування
**H₀ (нульова):** Використання методів машинного навчання (Random Forest, XGBoost) НЕ покращує точність прогнозування страхових ризиків порівняно з традиційними актуарними методами (GLM).

**H₁ (альтернативна):** Використання методів машинного навчання покращує точність прогнозування на статистично значущу величину (мінімум 5% покращення AUC-ROC).

**Метрики для перевірки:**
- AUC-ROC для класифікаційних моделей
- RMSE для регресійних моделей
- Статистичний тест: paired t-test на cross-validation scores

**Очікуваний результат:**
ML методи покажуть AUC-ROC = 0.85-0.90 проти 0.75-0.80 для традиційних методів.

---

### Гіпотеза 2: Телематичні дані
**H₀:** Додавання телематичних даних (швидкість, гальмування, час доби, маршрути) НЕ знижує похибку оцінки ризику.

**H₁:** Інтеграція реал-тайм телематичних даних знижує похибку прогнозування на 15-20% (зменшення RMSE).

**Дані для перевірки:**
- Baseline модель: тільки статичні дані (вік, стаж, характеристики авто)
- Enhanced модель: статичні + телематичні дані

**Метрики:**
- RMSE (Root Mean Squared Error)
- MAE (Mean Absolute Error)
- R² (coefficient of determination)

**Методологія:**
A/B тестування на контрольній та експериментальній групах клієнтів з телематичними пристроями.

---

### Гіпотеза 3: Персоналізація тарифів
**H₀:** Емпіричні моделі НЕ дозволяють персоналізувати страхові тарифи з точністю 90%+.

**H₁:** Використання емпіричних моделей дозволяє створювати персоналізовані тарифи, які точно відображають індивідуальний ризик у 90%+ випадків.

**Критерії точності:**
- Calibration plot: наскільки спрогнозовані ймовірності відповідають реальним частотам
- Brier score < 0.1 (міра точності ймовірнісних прогнозів)
- Відношення фактичних збитків до спрогнозованих: 0.95-1.05

**Бізнес-метрики:**
- Loss Ratio = Виплати / Премії (оптимально: 0.65-0.75)
- Утримання клієнтів (retention rate)
- Конкурентоспроможність цін

---

## 7. Архітектура програмного рішення

### 7.1. Компоненти системи

```
┌─────────────────────────────────────────────────────────┐
│                  Data Collection Layer                   │
│  - Historical claims data                                │
│  - Customer profiles                                     │
│  - Telematics API                                        │
│  - External data sources (weather, traffic)              │
└────────────────┬────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────┐
│                Data Processing Layer                     │
│  - Data cleaning (pandas)                                │
│  - Feature engineering                                   │
│  - Data validation                                       │
│  - ETL pipelines (Apache Airflow)                        │
└────────────────┬────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────┐
│              ML Model Training Layer                     │
│  - Model training (scikit-learn, XGBoost)                │
│  - Hyperparameter tuning (Optuna, GridSearchCV)          │
│  - Model validation (cross-validation)                   │
│  - MLOps (MLflow, Weights & Biases)                      │
└────────────────┬────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────┐
│              Model Serving Layer                         │
│  - REST API (FastAPI, Flask)                             │
│  - Model versioning                                      │
│  - A/B testing framework                                 │
│  - Monitoring and logging                                │
└────────────────┬────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────┐
│              Application Layer                           │
│  - Web interface                                         │
│  - Mobile app                                            │
│  - Integration with CRM/Policy systems                   │
└─────────────────────────────────────────────────────────┘
```

### 7.2. Технологічний стек

**Backend:**
- Python 3.10+
- FastAPI для API endpoints
- PostgreSQL/MySQL для реляційних даних
- MongoDB для профілів користувачів
- Redis для кешування

**ML/Data Science:**
- pandas, numpy для обробки даних
- scikit-learn для базових моделей
- XGBoost/LightGBM для production моделей
- statsmodels для актуарних розрахунків
- matplotlib, seaborn, plotly для візуалізації

**MLOps:**
- MLflow для tracking експериментів
- Docker для контейнеризації
- Kubernetes для оркестрації
- Apache Airflow для ETL пайплайнів

**Моніторинг:**
- Prometheus + Grafana
- ELK Stack (Elasticsearch, Logstash, Kibana)

---

## 8. Приклад коду: End-to-End pipeline

### 8.1. Завантаження та підготовка даних

```python
import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, LabelEncoder

# Завантаження даних
df = pd.read_csv('insurance_data.csv')

# Базовий огляд
print(df.head())
print(df.info())
print(df.describe())

# Обробка пропущених значень
df['years_of_experience'].fillna(df['years_of_experience'].median(), inplace=True)
df['previous_claims'].fillna(0, inplace=True)

# Feature Engineering
df['age_group'] = pd.cut(df['age'], bins=[18, 25, 35, 50, 70],
                          labels=['young', 'middle', 'mature', 'senior'])
df['risk_score'] = (df['previous_claims'] * 10 +
                    df['violations'] * 5 -
                    df['years_of_experience'] * 2)

# Кодування категоріальних змінних
le_gender = LabelEncoder()
df['gender_encoded'] = le_gender.fit_transform(df['gender'])

# One-hot encoding для багатокатегоріальних змінних
df = pd.get_dummies(df, columns=['car_brand', 'region'], drop_first=True)

# Розділення на features та target
X = df.drop(['claim_occurred', 'customer_id'], axis=1)
y = df['claim_occurred']

# Train/test split
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

# Стандартизація числових ознак
scaler = StandardScaler()
numerical_cols = ['age', 'years_of_experience', 'annual_mileage', 'car_value']
X_train[numerical_cols] = scaler.fit_transform(X_train[numerical_cols])
X_test[numerical_cols] = scaler.transform(X_test[numerical_cols])
```

### 8.2. Навчання моделей

```python
from sklearn.linear_model import LogisticRegression
from sklearn.ensemble import RandomForestClassifier
from xgboost import XGBClassifier
from sklearn.metrics import classification_report, roc_auc_score, roc_curve
import matplotlib.pyplot as plt

# 1. Логістична регресія (baseline)
lr_model = LogisticRegression(max_iter=1000, random_state=42)
lr_model.fit(X_train, y_train)
lr_pred = lr_model.predict(X_test)
lr_proba = lr_model.predict_proba(X_test)[:, 1]

print("Logistic Regression:")
print(classification_report(y_test, lr_pred))
print(f"AUC-ROC: {roc_auc_score(y_test, lr_proba):.4f}\n")

# 2. Random Forest
rf_model = RandomForestClassifier(n_estimators=200, max_depth=10,
                                   min_samples_split=20, random_state=42)
rf_model.fit(X_train, y_train)
rf_pred = rf_model.predict(X_test)
rf_proba = rf_model.predict_proba(X_test)[:, 1]

print("Random Forest:")
print(classification_report(y_test, rf_pred))
print(f"AUC-ROC: {roc_auc_score(y_test, rf_proba):.4f}\n")

# Feature importance
feature_importance = pd.DataFrame({
    'feature': X_train.columns,
    'importance': rf_model.feature_importances_
}).sort_values('importance', ascending=False)
print("Top 10 important features:")
print(feature_importance.head(10))

# 3. XGBoost (найкраща модель)
xgb_model = XGBClassifier(
    n_estimators=300,
    max_depth=6,
    learning_rate=0.05,
    subsample=0.8,
    colsample_bytree=0.8,
    gamma=1,
    reg_alpha=0.1,
    reg_lambda=1,
    random_state=42,
    eval_metric='auc'
)

xgb_model.fit(X_train, y_train,
              eval_set=[(X_test, y_test)],
              early_stopping_rounds=50,
              verbose=50)

xgb_pred = xgb_model.predict(X_test)
xgb_proba = xgb_model.predict_proba(X_test)[:, 1]

print("XGBoost:")
print(classification_report(y_test, xgb_pred))
print(f"AUC-ROC: {roc_auc_score(y_test, xgb_proba):.4f}\n")

# ROC Curve порівняння
plt.figure(figsize=(10, 6))
for name, proba in [('Logistic Regression', lr_proba),
                    ('Random Forest', rf_proba),
                    ('XGBoost', xgb_proba)]:
    fpr, tpr, _ = roc_curve(y_test, proba)
    auc = roc_auc_score(y_test, proba)
    plt.plot(fpr, tpr, label=f'{name} (AUC = {auc:.3f})')

plt.plot([0, 1], [0, 1], 'k--', label='Random')
plt.xlabel('False Positive Rate')
plt.ylabel('True Positive Rate')
plt.title('ROC Curves Comparison')
plt.legend()
plt.grid(True)
plt.savefig('roc_curves.png', dpi=300, bbox_inches='tight')
plt.show()
```

### 8.3. Розрахунок страхової премії (GLM)

```python
import statsmodels.api as sm
from statsmodels.genmod.families import Gamma, Tweedie

# Дані про розмір виплат (тільки для випадків з claim)
claims_data = df[df['claim_occurred'] == 1].copy()
X_claims = claims_data[['age', 'years_of_experience', 'car_value', 'previous_claims']]
y_claims = claims_data['claim_amount']

# GLM з Gamma розподілом для моделювання розміру збитків
X_claims_with_const = sm.add_constant(X_claims)
gamma_model = sm.GLM(y_claims, X_claims_with_const,
                     family=Gamma(link=sm.families.links.log()))
gamma_results = gamma_model.fit()
print(gamma_results.summary())

# Прогнозування розміру збитків
predicted_claim_amount = gamma_results.predict(X_claims_with_const)

# Розрахунок страхової премії
# Premium = P(claim) * E(claim_amount | claim occurred) * (1 + loading)
claim_probability = xgb_proba  # з моделі класифікації
expected_claim_amount = gamma_results.predict(sm.add_constant(X_test))

loading_factor = 1.35  # 35% навантаження (витрати + прибуток)
insurance_premium = claim_probability * expected_claim_amount * loading_factor

# Додавання базової премії
base_premium = 500  # мінімальна премія
final_premium = base_premium + insurance_premium

print(f"Average Premium: ${final_premium.mean():.2f}")
print(f"Premium Range: ${final_premium.min():.2f} - ${final_premium.max():.2f}")
```

### 8.4. Сегментація клієнтів

```python
from sklearn.cluster import KMeans, DBSCAN
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import silhouette_score
import matplotlib.pyplot as plt

# Підготовка даних для кластеризації
features_for_clustering = ['age', 'years_of_experience', 'annual_mileage',
                           'car_value', 'previous_claims', 'violations']
X_cluster = df[features_for_clustering].copy()

# Стандартизація (критично важливо для K-Means!)
scaler = StandardScaler()
X_cluster_scaled = scaler.fit_transform(X_cluster)

# Визначення оптимальної кількості кластерів (Elbow method)
inertias = []
silhouette_scores = []
K_range = range(2, 11)

for k in K_range:
    kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
    kmeans.fit(X_cluster_scaled)
    inertias.append(kmeans.inertia_)
    silhouette_scores.append(silhouette_score(X_cluster_scaled, kmeans.labels_))

# Візуалізація
fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(15, 5))

ax1.plot(K_range, inertias, 'bo-')
ax1.set_xlabel('Number of Clusters (K)')
ax1.set_ylabel('Inertia')
ax1.set_title('Elbow Method')
ax1.grid(True)

ax2.plot(K_range, silhouette_scores, 'ro-')
ax2.set_xlabel('Number of Clusters (K)')
ax2.set_ylabel('Silhouette Score')
ax2.set_title('Silhouette Analysis')
ax2.grid(True)

plt.tight_layout()
plt.savefig('cluster_optimization.png', dpi=300)
plt.show()

# Застосування K-Means з оптимальним K (припустимо, 6)
optimal_k = 6
kmeans_final = KMeans(n_clusters=optimal_k, random_state=42, n_init=10)
clusters = kmeans_final.fit_predict(X_cluster_scaled)

df['cluster'] = clusters

# Аналіз кластерів
cluster_summary = df.groupby('cluster')[features_for_clustering + ['claim_occurred']].agg([
    'mean', 'std', 'count'
])
print("\nCluster Summary:")
print(cluster_summary)

# Інтерпретація кластерів
cluster_names = {
    0: 'Low Risk - Young Professionals',
    1: 'Moderate Risk - Average Drivers',
    2: 'High Risk - Inexperienced',
    3: 'Premium - High Value Cars',
    4: 'Senior - Low Mileage',
    5: 'Risk Group - Many Claims'
}

df['cluster_name'] = df['cluster'].map(cluster_names)

# Візуалізація кластерів (PCA для 2D)
from sklearn.decomposition import PCA

pca = PCA(n_components=2)
X_pca = pca.fit_transform(X_cluster_scaled)

plt.figure(figsize=(12, 8))
scatter = plt.scatter(X_pca[:, 0], X_pca[:, 1], c=clusters, cmap='viridis',
                      alpha=0.6, edgecolors='k', s=50)
plt.xlabel(f'PC1 ({pca.explained_variance_ratio_[0]:.2%} variance)')
plt.ylabel(f'PC2 ({pca.explained_variance_ratio_[1]:.2%} variance)')
plt.title('Customer Segmentation (K-Means)')
plt.colorbar(scatter, label='Cluster')
plt.grid(True, alpha=0.3)
plt.savefig('clusters_visualization.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## 9. Висновки про необхідність емпіричних моделей

### 9.1. Ключові переваги емпіричних підходів

1. **Підвищення точності прогнозування:**
   - Традиційні актуарні таблиці базуються на обмежених факторах (вік, стать, регіон)
   - Моделі машинного навчання враховують десятки факторів та їх складні взаємодії
   - Очікуване покращення AUC-ROC з 0.75-0.80 до 0.85-0.90 (12-15% покращення)
   - Зменшення помилки прогнозування розміру збитків на 20-25%

2. **Персоналізація та справедливість:**
   - Кожен клієнт отримує тариф, що відповідає його індивідуальному профілю ризику
   - Безпечні водії платять менше, що стимулює відповідальну поведінку
   - Уникнення cross-subsidization (коли безпечні водії платять за ризикових)
   - Підвищення конкурентоспроможності завдяки точнішому ціноутворенню

3. **Адаптивність до змін:**
   - Моделі можна регулярно перенавчати на нових даних
   - Автоматичне виявлення нових патернів та трендів
   - Швидка реакція на зміни в ринку (нові типи автомобілів, зміна поведінки водіїв)
   - Integration телематичних даних для real-time оцінки ризику

4. **Оптимізація бізнес-процесів:**
   - Автоматизація underwriting процесу
   - Зменшення часу на розгляд заявок з днів до хвилин
   - Виявлення шахрайства на ранніх стадіях
   - Покращення утримання клієнтів (retention) завдяки справедливим цінам

### 9.2. Виклики та обмеження

1. **Якість даних:**
   - Необхідність великих обсягів якісних історичних даних
   - Проблема missing values та неточних даних
   - Необхідність постійного моніторингу якості вхідних даних

2. **Інтерпретованість:**
   - Складні моделі (XGBoost, нейронні мережі) є "чорними скриньками"
   - Регуляторні вимоги щодо пояснюваності рішень
   - Необхідність балансу між точністю та інтерпретованістю

3. **Етичні питання:**
   - Ризик дискримінації за певними ознаками (вік, стать, район проживання)
   - Необхідність Fairness-aware ML підходів
   - Відповідність GDPR та іншим регуляторним вимогам

4. **Технічна складність:**
   - Потреба в кваліфікованих data scientists та ML engineers
   - Інфраструктура для обробки великих даних
   - Постійне оновлення та моніторинг моделей

### 9.3. Рекомендації щодо впровадження

1. **Поетапний підхід:**
   - Почати з простих моделей (логістична регресія) як baseline
   - Поступово переходити до складніших (Random Forest, XGBoost)
   - Постійно порівнювати з традиційними методами

2. **Hybrid підхід:**
   - Комбінація актуарної експертизи та ML моделей
   - Використання GLM для базових розрахунків + ML для покращення
   - Експертна валідація результатів моделей

3. **Моніторинг та оновлення:**
   - Регулярне перенавчання моделей (quarterly або bi-annually)
   - Моніторинг model drift та performance degradation
   - A/B тестування нових моделей перед повним впровадженням

4. **Документація та governance:**
   - Повна документація моделей для регуляторів
   - Model cards з описом даних, методології, обмежень
   - Процеси для appeal та перегляду рішень

### 9.4. Загальний висновок

Використання емпіричних моделей та методів машинного навчання у розробці систем розрахунку страхового ризику автомобіля є **критично необхідним** для сучасних страхових компаній. Це дозволяє:

- Підвищити точність оцінки ризику на 15-20%
- Персоналізувати тарифи для мільйонів клієнтів
- Автоматизувати процеси та зменшити операційні витрати
- Покращити customer experience
- Залишатися конкурентоспроможними на ринку

Однак впровадження повинно бути продуманим, етичним та відповідати регуляторним вимогам. Найкращі результати досягаються при комбінації традиційної актуарної експертизи з сучасними методами data science.

---

## 10. Список використаних джерел

1. Hastie, T., Tibshirani, R., & Friedman, J. (2009). The Elements of Statistical Learning: Data Mining, Inference, and Prediction. Springer.

2. Wüthrich, M. V., & Buser, C. (2021). Data Analytics for Non-Life Insurance Pricing. Swiss Finance Institute Research Paper.

3. Frees, E. W., Derrig, R. A., & Meyers, G. (2014). Predictive Modeling Applications in Actuarial Science. Cambridge University Press.

4. Chen, T., & Guestrin, C. (2016). XGBoost: A Scalable Tree Boosting System. Proceedings of the 22nd ACM SIGKDD International Conference.

5. Scikit-learn Documentation: https://scikit-learn.org/stable/

6. XGBoost Documentation: https://xgboost.readthedocs.io/

7. Statsmodels Documentation: https://www.statsmodels.org/

8. Insurance Telematics Report (2024). McKinsey & Company.

---

**Виконав:** Студент групи [ваша група]
**Дата:** 15 листопада 2025
**Викладач:** [ПІБ викладача]
