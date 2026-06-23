FROM python:3.11-slim

# منع إنشاء ملفات pyc وإظهار السجلات مباشرة
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

WORKDIR /app

# تثبيت الاعتماديات
COPY requirements.txt .

RUN pip install --no-cache-dir -r requirements.txt

# نسخ المشروع
COPY . .

# المنفذ المطلوب في Hugging Face Spaces
EXPOSE 7860

# تشغيل FastAPI
CMD ["uvicorn", "app:app", "--host", "0.0.0.0", "--port", "7860"]