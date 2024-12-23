# Use NVIDIA's CUDA base image
FROM nvidia/cuda:12.6.3-devel-ubuntu24.04

# Set environment variables to prevent interactive prompts during package installation
ENV DEBIAN_FRONTEND=noninteractive

# Install Python and pip
RUN apt-get update && apt-get install -y \
    python3 \
    python3-pip \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Install PyTorch (CUDA-enabled version)
RUN pip install --break-system-packages --no-cache-dir --pre torch torchvision torchaudio --index-url https://download.pytorch.org/whl/nightly/cu126

# Copy the test script into the container
COPY TestGPU.py /workspace/TestGPU.py

# Set the working directory
WORKDIR /workspace

# Set default command to run the script
CMD ["python3", "TestGPU.py"]
