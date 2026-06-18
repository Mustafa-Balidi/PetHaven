# استخدام Node.js
FROM node:22-alpine

# مجلد العمل داخل الحاوية
WORKDIR /app

# نسخ ملفات الاعتماد
COPY package*.json ./

# تثبيت الحزم
RUN npm install

# نسخ بقية المشروع
COPY . .

# المنفذ الذي يستخدمه Vite
EXPOSE 5173

# تشغيل Vite مع السماح بالاتصال من خارج الحاوية
CMD ["npm", "run", "dev", "--", "--host"]