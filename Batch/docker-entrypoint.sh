#!/bin/bash

# Lambdaのランタイムの仕様に適合させる
while true
do
    # 関数開始リクエストを送信してリクエストIDを取得する
    HEADERS="$(mktemp)"
    curl -sS -LD "$HEADERS" -X GET "http://${AWS_LAMBDA_RUNTIME_API}/2018-06-01/runtime/invocation/next"
    REQUEST_ID=$(grep -Fi Lambda-Runtime-Aws-Request-Id "$HEADERS" | tr -d '[:space:]' | cut -d: -f2)

    # LambdaのCMDを起動する
    dotnet Batch.dll $@

    # 関数終了をLambdaランタイムに通知する
    curl -X POST "http://${AWS_LAMBDA_RUNTIME_API}/2018-06-01/runtime/invocation/$REQUEST_ID/response"  -d "SUCCESS"
done