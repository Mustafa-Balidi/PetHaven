FROM python:3.11-slim

WORKDIR /app

# نسخ ملف المتطلبات
COPY requirements.txt .

# تثبيت المكتبات
RUN pip install --no-cache-dir -r requirements.txt

# نسخ بقية الملفات
COPY . .

# المنفذ
EXPOSE 8000

# تشغيل FastAPI
CMD ["uvicorn", "app:app", "--host", "0.0.0.0", "--port", "8000"]