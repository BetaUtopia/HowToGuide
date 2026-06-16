# Install Docker Engine

# Get Nginx Proxy Manager and Portainer
```bash
git clone --no-checkout https://github.com/BetaUtopia/HowToGuide.git && \
cd HowToGuide && \
git sparse-checkout init --cone && \
git sparse-checkout set Docker_NginxProxyManager_Portainer && \
git checkout
```

## Uninstall all conflicting

```bash
sudo apt remove $(dpkg --get-selections docker.io docker-compose docker-compose-v2 docker-doc podman-docker containerd runc | cut -f1)
```


## Add Docker's official GPG key:

```bash
sudo apt update
sudo apt install ca-certificates curl
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc
```


## Add the repository to Apt sources:
```bash
sudo tee /etc/apt/sources.list.d/docker.sources <<EOF
Types: deb
URIs: https://download.docker.com/linux/ubuntu
Suites: $(. /etc/os-release && echo "${UBUNTU_CODENAME:-$VERSION_CODENAME}")
Components: stable
Architectures: $(dpkg --print-architecture)
Signed-By: /etc/apt/keyrings/docker.asc
EOF
```

```bash
sudo apt update
```

```bash
install the latest version
 sudo apt install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```

```bash
sudo systemctl status docker
```

```bash
sudo systemctl start docker
```

# Create Local Network
```bash
sudo docker network create localnet
```

# Create .wslconfig File
C:\Users\<Your-Username>\.wslconfig


```powershell
notepad $env:USERPROFILE\.wslconfig
```

```
[wsl2]
networkingMode=mirrored

[experimental]
hostAddressLoopback=true
```

```powershell
wsl --shutdown
```

```powershell
wsl -d Ubuntu-26.04
```

# 1. Permanent rule for the main Windows Firewall
```powershell
New-NetFirewallRule -DisplayName "Nginx Proxy Manager LAN Access" -Direction Inbound -LocalPort 80,81,443 -Protocol TCP -Action Allow -Profile Private,Public -ErrorAction SilentlyContinue
```

# 2. Permanent rule for the Hyper-V WSL Switch
```powershell
$vmid = (Get-NetFirewallHyperVVMCreator | Where-Object {$_.FriendlyName -match "WSL"}).VMCreatorId
New-NetFirewallHyperVRule -DisplayName "Nginx Proxy Manager Inbound" -VMCreatorId $vmid -Direction Inbound -Protocol TCP -LocalPort 80,443,81 -Action Allow -ErrorAction SilentlyContinue
```

```powershell
ipconfig /flushdns
```

# Create SSL Cert
```bash
./CreateCRT.sh
```

# Ubuntu Location from Windows File Explorer
\\wsl.localhost\Ubuntu-26.04
