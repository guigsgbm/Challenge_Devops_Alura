FROM python:3.8-alpine
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1
WORKDIR /code
COPY requirements.txt /code/
#RUN apk update
#RUN apk add postgresql-dev gcc python3-dev musl-dev
#RUN pip install psycopg2-binary
RUN pip install -r requirements.txt --no-cache
COPY . /code/
