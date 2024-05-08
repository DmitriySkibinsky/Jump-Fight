FROM alpine:latest

RUN apk add --no-cache unzip

RUN mkdir /output

COPY ./Jump-Fight.zip /tmp/

RUN unzip /tmp/Jump-Fight.zip -d /output

CMD ["sh", "-c", "mkdir -p /host/output && cp -r /output/. /host/output"]
