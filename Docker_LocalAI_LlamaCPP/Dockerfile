# Stage 1: Build
FROM nvidia/cuda:13.1.0-devel-ubuntu24.04 AS builder

# Added libssl-dev for HTTPS/OpenSSL support
RUN apt-get update && apt-get install -y \
    build-essential cmake git libcurl4-openssl-dev libssl-dev pkg-config \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
RUN git clone https://github.com/ggerganov/llama.cpp.git .

# Explicitly enable OpenSSL and CURL
RUN cmake -B build \
    -DGGML_CUDA=ON \
    -DLLAMA_CURL=ON \
    -DLLAMA_OPENSSL=ON \
    -DCMAKE_CUDA_ARCHITECTURES=native \
    -DCMAKE_BUILD_TYPE=Release \
    -DBUILD_SHARED_LIBS=OFF \
    -DLLAMA_STATIC=ON

# -j 2 to stay safe on memory
RUN cmake --build build --config Release -j 2

# Stage 2: Runtime
FROM nvidia/cuda:13.1.0-runtime-ubuntu24.04

# Added openssl and ca-certificates to handle HTTPS at runtime
RUN apt-get update && apt-get install -y \
    libgomp1 \
    libcurl4 \
    openssl \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /llama
COPY --from=builder /app/build/bin/llama-server .
COPY --from=builder /app/build/bin/llama-cli .


EXPOSE 8080
ENTRYPOINT ["./llama-server"]