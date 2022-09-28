FROM python:3.8-slim
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1
WORKDIR /code
COPY requirements.txt /code/
RUN apk update \
apk add postgresql-dev gcc python3-dev musl-dev && \
pip install psycopg2-binary && \
pip install -r requirements.txt
COPY . /code/