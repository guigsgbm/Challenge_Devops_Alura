FROM python:3.8
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1
WORKDIR /code
COPY requirements.txt /code/
RUN pip install psycopg2-binary && \
pip install -r requirements.txt --no-cache
COPY . /code/