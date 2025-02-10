# Setup Kubernetes Cluster on Raspbery Pi
___

## Connect To Wifi

```bash
sudo nano /etc/netplan/00-installer-config.yaml
```

```bash
sudo netplan apply
```

## Set Static IP
```bash
cd /etc/netplan/
ls
sudo nano /etc/netplan/50-cloud-init.yaml
```
01-netcfg.yaml, 50-cloud-init.yaml, or NN_interfaceName.yaml


# (Master and Node)
___
## Update Firmware

```bash
sudo apt update && sudo apt full-upgrade -y
sudo rpi-eeprom-update -a
```

```bash
sudo reboot
```

If you want to override how much cooling is done on the Pi
```bash
echo 2 | sudo tee /sys/class/thermal/cooling_device0/cur_state
```

## Get Files
```bash
git clone --no-checkout https://github.com/BetaUtopia/HowToGuide.git && \
cd HowToGuide && \
git sparse-checkout init --cone && \
git sparse-checkout set Kubernetes_K8s_PI5_Cluster && \
git checkout
```

# Kubernetes Prerequisites
___
## Update and Install Packages
```bash
sudo apt update && sudo apt upgrade -y
sudo apt install -y apt-transport-https ca-certificates curl net-tools gnupg
sudo apt-get update
```

## Set Nano as Default Editor
```bash
echo "export EDITOR=nano" >> ~/.bashrc
echo "export VISUAL=nano" >> ~/.bashrc
source ~/.bashrc
```

## Enable IPv4 packet forwarding

### sysctl params required by setup, params persist across reboots
```bash
cat <<EOF | sudo tee /etc/sysctl.d/k8s.conf
net.ipv4.ip_forward = 1
EOF
```

### Apply sysctl params without reboot
```bash
sudo sysctl --system
```

## Install runc
```bash
curl -LO https://github.com/opencontainers/runc/releases/download/v1.2.4/runc.arm64
chmod +x runc.arm64
sudo mv runc.arm64 /usr/local/sbin/runc
runc --version
```

## Install CNI
```bash
curl -LO https://github.com/containernetworking/plugins/releases/download/v1.6.2/cni-plugins-linux-arm-v1.6.2.tgz
sudo mkdir -p /opt/cni/bin
sudo tar -C /opt/cni/bin -xzf cni-plugins-linux-arm-v1.6.2.tgz
ls /opt/cni/bin
rm cni-plugins-linux-arm-v1.6.2.tgz
```

## Install containerd
```bash
curl -LO https://github.com/containerd/containerd/releases/download/v2.0.2/containerd-2.0.2-linux-arm64.tar.gz
tar -xvzf containerd-2.0.2-linux-arm64.tar.gz
sudo mv bin/* /usr/local/bin/
containerd --version
rm containerd-2.0.2-linux-arm64.tar.gz
rm -rf bin
```

## Install containerd Service
```bash
sudo curl -o /etc/systemd/system/containerd.service https://raw.githubusercontent.com/containerd/containerd/main/containerd.service
systemctl daemon-reload
systemctl enable --now containerd
sudo systemctl enable containerd
sudo systemctl restart containerd
```

### Install Firewall
```bash
sudo apt update
sudo apt install ufw -y
```

### Firewall (Master ONLY)
```bash
sudo ufw allow 22/tcp
sudo ufw allow 8443/tcp
sudo ufw allow 80/tcp
sudo ufw allow 6443/tcp       # Kubernetes API server
sudo ufw allow 2379:2380/tcp  # etcd server client API
sudo ufw allow 10250/tcp      # Kubelet API
sudo ufw allow 10259/tcp      # kube-scheduler
sudo ufw allow 10257/tcp      # kube-controller-manager
sudo ufw allow 5000/tcp       # Docker Registry
sudo ufw allow 30001/tcp      # Docker Registry External
```

### Firewall (Node ONLY)
```bash
sudo ufw allow 22/tcp
sudo ufw allow 10250/tcp        # Kubelet API
sudo ufw allow 10256/tcp        # kube-proxy
sudo ufw allow 30000:32767/tcp  # NodePort Services
```

```bash
sudo ufw enable
```

## Disable Swap
```bash
sudo nano /etc/fstab
```
UUID=xxxx-xxxx-xxxx-xxxx  none  swap  sw  0  0

## Install kubectl
```bash
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/arm64/kubectl"
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/arm64/kubectl.sha256"
echo "$(cat kubectl.sha256)  kubectl" | sha256sum --check
```
## kubectl
```bash
sudo install -o root -g root -m 0755 kubectl /usr/local/bin/kubectl
rm kubectl
rm kubectl.sha256
```

```bash
kubectl version --client
kubectl version --client --output=yaml 
```

```bash
sudo mkdir -p -m 755 /etc/apt/keyrings
curl -fsSL https://pkgs.k8s.io/core:/stable:/v1.32/deb/Release.key | sudo gpg --dearmor -o /etc/apt/keyrings/kubernetes-apt-keyring.gpg
sudo chmod 644 /etc/apt/keyrings/kubernetes-apt-keyring.gpg 
```
This overwrites any existing configuration in /etc/apt/sources.list.d/kubernetes.list
```bash
echo 'deb [signed-by=/etc/apt/keyrings/kubernetes-apt-keyring.gpg] https://pkgs.k8s.io/core:/stable:/v1.32/deb/ /' | sudo tee /etc/apt/sources.list.d/kubernetes.list
sudo chmod 644 /etc/apt/sources.list.d/kubernetes.list   
```
helps tools such as command-not-found to work correctly

```bash
sudo apt-get update
sudo apt-get install -y kubelet kubeadm kubectl
```

# Install K8s Kubernetes
```bash
curl -fsSL https://pkgs.k8s.io/core:/stable:/v1.32/deb/Release.key | sudo gpg --dearmor -o /etc/apt/keyrings/kubernetes-apt-keyring.gpg
```
```bash
echo 'deb [signed-by=/etc/apt/keyrings/kubernetes-apt-keyring.gpg] https://pkgs.k8s.io/core:/stable:/v1.32/deb/ /' | sudo tee /etc/apt/sources.list.d/kubernetes.list
```
```bash
sudo systemctl enable --now kubelet
```

```bash
sudo reboot
```

```bash
sudo kubeadm config images pull
```

# End of Node (Master Only)
___

## Initializing your control-plane (Master Only)
```bash
sudo kubeadm init \
  --control-plane-endpoint=192.168.86.100:6443 \
  --cri-socket=unix:///var/run/containerd/containerd.sock
```

```bash
sudo chown $USER:$USER /etc/kubernetes/admin.conf
```

```bash
mkdir -p $HOME/.kube
sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
sudo chown $(id -u):$(id -g) $HOME/.kube/config
export KUBECONFIG=/etc/kubernetes/admin.conf
```

```bash
kubectl get nodes
kubectl cluster-info
```

## Install Calico
```bash
kubectl create -f https://raw.githubusercontent.com/projectcalico/calico/v3.29.1/manifests/tigera-operator.yaml
kubectl taint nodes --all node-role.kubernetes.io/control-plane-
```

```bash
kubectl apply -f https://docs.projectcalico.org/manifests/calico.yaml
```

### Check all Pods
```bash
watch kubectl get pods --all-namespaces
```
wait for all to be running

### Get Detailed Pod Information
```bash
kubectl describe pod <pod-name> -n <namespace>
kubectl describe pod kube-apiserver-k8smaster -n kube-system
kubectl get nodes -o wide
```

## Install Helm
```bash
curl -fsSL -o get_helm.sh https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3
chmod 700 get_helm.sh
./get_helm.sh
helm version
```

## Install Ingress
___
```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.12.0/deploy/static/provider/cloud/deploy.yaml
```

## Install Dashboard
___
### Add kubernetes-dashboard repository
```bash
helm repo add kubernetes-dashboard https://kubernetes.github.io/dashboard/
```
### Deploy a Helm Release named "kubernetes-dashboard" using the kubernetes-dashboard chart

```bash
helm upgrade --install kubernetes-dashboard kubernetes-dashboard/kubernetes-dashboard --create-namespace --namespace kubernetes-dashboard
```

### Wait for Pod to start
```bash
watch kubectl get pods --all-namespaces
```

## Expose the Dashboard
```bash
kubectl edit svc kubernetes-dashboard-kong-proxy -n kubernetes-dashboard
```

type: ClusterIP -> NodePort

```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-ingress.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-ingress.yaml
```

```bash
kubectl expose deployment ingress-nginx-controller --type=NodePort --name=ingress-nginx --namespace=ingress-nginx
kubectl get svc -n ingress-nginx
```

# On Windows Host
___
C:\Windows\System32\drivers\etc\hosts
Client Edit /etc/hosts
Add 
```bash
192.168.86.100  dashboard.local
192.168.86.100  k8smaster
```

```bash
sudo nano /etc/hosts
```

### Edit/Validate the Ingress resource
```bash
kubectl edit ingress kubernetes-dashboard -n kubernetes-dashboard
```
nginx.ingress.kubernetes.io/whitelist-source-range: 0.0.0.0/0


## Create Service Account for Dashboard User to use
___
### Get all service accounts
```bash
kubectl get serviceaccounts --all-namespaces
```
### Create the service account
```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-user.yaml
```
### Apply the user:
```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-user.yaml
```
### Create Role Binding
```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-clusterrolebinding.yaml
```
### Apply Role Binding
```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-clusterrolebinding.yaml
```
### Retrieve the bearer token and copy the output for later use
```bash
kubectl get secret $(kubectl get serviceaccount dashboard-user -n kubernetes-dashboard -o jsonpath="{.secrets[0].name}") -n kubernetes-dashboard -o jsonpath="{.data.token}" | base64 --decode 
```

### Allow access with read-only user
```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-user-role.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-user-role.yaml
```

### Role Binding
```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-user-role-binding.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/dashboard/dashboard-user-role-binding.yaml
```

### Retrieve the bearer token and copy the output for later use
```bash
kubectl get secret $(kubectl get serviceaccount dashboard-user -n kubernetes-dashboard -o jsonpath="{.secrets[0].name}") -o jsonpath="{.data.token}" | base64 --decode
```

```bash
kubectl create token dashboard-user -n kubernetes-dashboard
```

# Generate the Self-Signed Certificate and Key
___

### Create a location to store the certificate
```bash
mkdir -p /home/pi/certs
```

### Create the certificate.
```bash
openssl req -x509 -newkey rsa:2048 -keyout /home/pi/certs/dashboard.key -out /home/pi/certs/dashboard.crt -days 365 -nodes \
  -subj "/CN=dashboard.local" \
  -config <(cat <<EOF
[req]
distinguished_name=req
x509_extensions = v3_req
[req_distinguished_name]
[v3_req]
subjectAltName=DNS:dashboard.local
EOF
)
```

### Create a kubernetes secret for holding the certificate and private key

```bash
kubectl create secret tls kubernetes-dashboard-certs --cert=/home/pi/certs/dashboard.crt --key=/home/pi/certs/dashboard.key -n kubernetes-dashboard
```

### Ensure the paths are correct
```bash
ls -l /home/pi/certs/
kubectl get secrets -n kubernetes-dashboard
```

```bash
kubectl patch ingress kubernetes-dashboard -n kubernetes-dashboard \
  -p '{"spec":{"tls":[{"hosts":["dashboard.local"],"secretName":"kubernetes-dashboard-certs"}]}}'
```

```bash
kubectl rollout restart deployment/ingress-nginx-controller -n ingress-nginx
```

```bash
kubectl logs -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx
```

```bash
kubectl get svc -n ingress-nginx
```

### ingress-nginx-controller Port 32371


### On Windows Machine
```bash
 scp pi@192.168.86.100:/home/pi/certs/dashboard.crt .
```

https://dashboard.local:30325/dashboard

# Create Token for Dashboard
```bash
kubectl create token dashboard-user -n kubernetes-dashboard
```

# Registry For Docker
___
```bash
  sudo mkdir -p /var/lib/registry
  sudo chown -R 1000:1000 /var/lib/registry
```

### namespace
```bash
kubectl create namespace registry
```

### pv

```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-pv.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-pv.yaml
```

### pvc

```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-pvc.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-pvc.yaml
```

### service

```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-service.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-service.yaml
```

```bash
openssl rand -base64 32
```

#  SSL for Registry
___
```bash
openssl req -x509 -newkey rsa:4096 -keyout /home/pi/certs/registry.key -out /home/pi/certs/registry.crt -days 365 -nodes \
  -subj "/CN=k8smaster" \
  -config <(cat <<EOF
[req]
distinguished_name=req
x509_extensions = v3_req
[req_distinguished_name]
[v3_req]
subjectAltName=DNS:k8smaster,DNS:localhost,IP:127.0.0.1
EOF
)
```

Both on Windows and WSL
```bash
sudo cp /home/pi/certs/registry.crt /usr/local/share/ca-certificates/k8smaster.crt
sudo update-ca-certificates
sudo ls /etc/ssl/certs | grep k8smaster
```

scp pi@192.168.86.100:/home/pi/certs/registry.crt .

```bash
kubectl create secret tls registry-tls \
  --cert=/home/pi/certs/registry.crt \
  --key=/home/pi/certs/registry.key \
  --namespace=registry
```

```bash
sudo cp registry.crt /usr/local/share/ca-certificates/k8smaster.crt
sudo update-ca-certificates
sudo ls /etc/ssl/certs | grep k8smaster
```

### deployment
```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-deployment.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-deployment.yaml
```

### ingress
```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-ingress.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/registry/registry-ingress.yaml
```

```bash
sudo mkdir -p /etc/containerd/
sudo nano /etc/containerd/config.toml
```

```bash
sudo systemctl restart containerd
```

## Test Registory from Browser

```bash
watch kubectl get pods --all-namespaces
```
Wait for it to be running

```bash
kubectl get svc -n registry
```

https://192.168.86.100:30001/v2/_catalog
https://K8sMaster:30001/v2/_catalog

# Setup Docker Engine
On Other Machine (Linux or Windows WSL)

### Install required dependencies
```bash
sudo apt-get update
sudo apt-get install apt-transport-https ca-certificates curl software-properties-common
```

### Add Docker’s official GPG key
```bash
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
```

### Add Docker repository
```bash
echo "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
```

### Update package index
```bash
sudo apt-get update
```

### Install Docker
```bash
sudo apt-get install docker-ce
```

### Start Docker service
```bash
sudo systemctl start docker
```

### Enable Docker to start on boot
```bash
sudo systemctl enable docker
```

### Verify Docker Installation
```bash
docker --version
```

### Multi-Platform Emulator
```bash
sudo docker run --privileged --rm tonistiigi/binfmt --install all
```

### Restart Docker
```bash
sudo service docker restart
```

# Public Docker image to Repository
### From Other PC

```bash
sudo nano  /etc/docker/daemon.json
```

```bash
sudo systemctl restart docker
```

```bash
docker pull --platform linux/arm64 arm64v8/alpine
docker tag arm64v8/hello-world 192.168.86.100:30001/alpine
docker push 192.168.86.100:30001/alpine
```

## Get Files
```bash
git clone --no-checkout https://github.com/BetaUtopia/HowToGuide.git && \
cd HowToGuide && \
git sparse-checkout init --cone && \
git sparse-checkout set Kubernetes_K8s_PI5_Cluster && \
git checkout
```

```bash
sudo docker buildx build --platform linux/arm64 -t 192.168.86.100:30001/alpine .
sudo docker push 192.168.86.100:30001/alpine
```

### Visit 
http://192.168.86.100:30001/v2/_catalog


# Deploy Apps to Kubernetes
___

```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/app/app-deployment.yaml
```

```bash
sudo nano HowToGuide/Kubernetes_K8s_PI5_Cluster/app/app-service.yaml
```

```bash
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/app/app-deployment.yaml
kubectl apply -f HowToGuide/Kubernetes_K8s_PI5_Cluster/app/app-service.yaml
```

```bash
kubectl get deployments
kubectl get pods
```

# Remove Image From Registry (If Needed)
```bash
curl -s -k -X GET https://k8smaster:30001/v2/alpine/manifests/latest \
     -H "Accept: application/vnd.docker.distribution.manifest.v2+json"
```
```bash
registry='k8smaster:30001'
name='alpine'
digest='sha256:dd5fee929cb56b8b99480d4c243c3f8a0f73f88e4941dba98ddbc8c9349b60aa'
curl -v -sSL -X DELETE "https://${registry}/v2/${name}/manifests/${digest}"
```
```bash
curl -s -k "https://k8smaster:30001/v2/alpine/tags/list"
```

# Check all Pods (If Needed)

```bash
kubectl get pods --all-namespaces
```

# Get Detailed Pod Information (If Needed)

```bash
kubectl describe pod <pod-name> -n <namespace>
```
```bash
kubectl describe pod kube-apiserver-k8smaster -n kube-system
```
```bash
kubectl get nodes -o wide
```

# Master ONLY
```bash
kubeadm token create --print-join-command
```

```bash
kubectl get nodes
```

# Node ONLY

```bash
cat <<EOF | sudo tee /etc/sysctl.d/k8s.conf
net.ipv4.ip_forward = 1
EOF
```
### Apply sysctl params without reboot
```bash
sudo sysctl --system
```

```bash
mkdir -p ~/.kube
scp pi@192.168.86.100:/etc/kubernetes/admin.conf ~/.kube/config
```
```bash
sudo chown $(id -u):$(id -g) $HOME/.kube/config
export KUBECONFIG=/etc/kubernetes/admin.conf
```
```bash
echo "export KUBECONFIG=~/.kube/config" >> ~/.bashrc
source ~/.bashrc
```

Example Only 
```bash
sudo kubeadm join 192.168.86.100:6443 --token k7drg9.gf3oaw9s72jvmevb --discovery-token-ca-cert-hash sha256:1b2dc867a04688d204009d5b7d4ee5b55d9c2a446a3f837c95756bab999dbe75
```

```bash
kubectl get nodes
```