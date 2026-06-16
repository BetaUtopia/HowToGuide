#!/bin/bash
TARGET_DIR="/home/sam/local-certs"
mkdir -p "$TARGET_DIR" && cd "$TARGET_DIR"
echo "Generating Root CA Key..."
openssl genrsa -out localCA.key 4096
echo "Generating Root CA Certificate..."
openssl req -x509 -new -nodes -key localCA.key -sha256 -days 3650 -subj "/C=US/ST=State/L=City/O=Local Network/CN=Local Root CA" -out localCA.crt
echo "Generating Wildcard Key..."
openssl genrsa -out wildcard.key 2048
echo "Creating explicit.ext file..."
printf "authorityKeyIdentifier=keyid,issuer\nbasicConstraints=CA:FALSE\nkeyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment\nsubjectAltName = @alt_names\n\n[alt_names]\nDNS.1 = localnet\nDNS.2 = *.localnet\n" > explicit.ext
echo "Generating Wildcard CSR..."
openssl req -new -key wildcard.key -subj "/C=US/ST=State/L=City/O=Local Network/CN=*.localnet" -out wildcard.csr
echo "Signing Wildcard Certificate..."
openssl x509 -req -in wildcard.csr -CA localCA.crt -CAkey localCA.key -CAcreateserial -out wildcard.crt -days 1095 -sha256 -extfile explicit.ext
echo "Done! Certs are located in $TARGET_DIR"
