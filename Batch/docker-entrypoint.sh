#!/bin/bash

# Lambda�̃����^�C���̎d�l�ɓK��������
while true
do
    # �֐��J�n���N�G�X�g�𑗐M���ă��N�G�X�gID���擾����
    HEADERS="$(mktemp)"
    curl -sS -LD "$HEADERS" -X GET "http://${AWS_LAMBDA_RUNTIME_API}/2018-06-01/runtime/invocation/next"
    REQUEST_ID=$(grep -Fi Lambda-Runtime-Aws-Request-Id "$HEADERS" | tr -d '[:space:]' | cut -d: -f2)

    # Lambda��CMD���N������
    dotnet Batch.dll $@

    # �֐��I����Lambda�����^�C���ɒʒm����
    curl -X POST "http://${AWS_LAMBDA_RUNTIME_API}/2018-06-01/runtime/invocation/$REQUEST_ID/response"  -d "SUCCESS"
done