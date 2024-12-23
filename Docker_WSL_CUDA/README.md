# Setup Docker Engine and WSL 2 Ubuntu with CUDA Enabled

## Install WSL 2 And Ubuntu 24.04 From Powershell 7

```bat
wsl --install
```

```bat
wsl.exe --list --online
```

```bat
wsl.exe --install Ubuntu-24.04
```

## Ubuntu 24.04

```bash
for pkg in docker.io docker-doc docker-compose docker-compose-v2 podman-docker containerd runc; do sudo apt-get remove $pkg; done
```

```bash
git clone --no-checkout https://github.com/BetaUtopia/HowToGuide.git && \
cd HowToGuide && \
git sparse-checkout init --cone && \
git sparse-checkout set Docker_WSL_CUDA && \
git checkout
```

```bash
./Docker_WSL_CUDA/SetupDockerRepository.sh
```

```bash
sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```

```bash
sudo docker run hello-world
```

### Create Docker Group , Add User To Group, Activate Changes
```bash
sudo groupadd docker
```

```bash
sudo usermod -aG docker $USER
```

```bash
 newgrp docker
```

### Enable Docker at Boot
```bash
sudo systemctl enable docker.service
```

```bash
sudo systemctl enable containerd.service
```

## Installing the NVIDIA Container Toolkit

```bash
nvidia-smi
```

```bash
wget https://developer.download.nvidia.com/compute/cuda/repos/ubuntu2404/x86_64/cuda-keyring_1.1-1_all.deb
```

```bash
sudo dpkg -i cuda-keyring_1.1-1_all.deb
```

### Configure Repository, Packages, And Update Repository:

```bash
curl -fsSL https://nvidia.github.io/libnvidia-container/gpgkey | sudo gpg --dearmor -o /usr/share/keyrings/nvidia-container-toolkit-keyring.gpg \
  && curl -s -L https://nvidia.github.io/libnvidia-container/stable/deb/nvidia-container-toolkit.list | \
    sed 's#deb https://#deb [signed-by=/usr/share/keyrings/nvidia-container-toolkit-keyring.gpg] https://#g' | \
    sudo tee /etc/apt/sources.list.d/nvidia-container-toolkit.list
```

```bash
sudo sed -i -e '/experimental/ s/^#//g' /etc/apt/sources.list.d/nvidia-container-toolkit.list
```

```bash
sudo apt-get update
```

## Install the NVIDIA Container Toolkit:

```bash
sudo apt-get install -y nvidia-container-toolkit
```

### Configure For Docker
```bash
sudo nvidia-ctk runtime configure --runtime=docker
```
```bash
sudo systemctl restart docker
```
