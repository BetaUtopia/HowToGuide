# Setup Docker Engine and WSL 2 Ubuntu with Juypter Lab 

Enter WSL2 / Ubuntu

```bash
ubuntu2404
```

## On WSL2 / Ubuntu

```bash
git clone --no-checkout https://github.com/BetaUtopia/HowToGuide.git && \
cd HowToGuide && \
git sparse-checkout init --cone && \
git sparse-checkout set Docker_JuypterLab && \
git checkout
```

```bash
echo "nameserver 8.8.8.8" | sudo tee /etc/resolv.conf > /dev/null
```

```bash
cd Docker_JuypterLab
```

```bash
docker rm -f jupyterlab && \
docker build -t jupyterlab_img . && \
docker run -d --gpus all --restart always --name jupyterlab \
  -p 8888:8888 \
  jupyterlab_img
```

```bash
docker exec -it jupyterlab /bin/bash
```

```bash
jupyter lab list
```
Copy the token and go to [localhost:8888 ](http://localhost:8888/login)

## On Windows Machine

Where files are located.
```bat
\\wsl.localhost\Ubuntu-24.04\home\sam\HowToGuide\Docker_JuypterLab
```

```bat
wsl hostname -I
```

```bat
netsh interface portproxy add v4tov4 listenport=8888 listenaddress=0.0.0.0 connectport=8888 connectaddress=172.17.0.1
```

## On Juypter Lab

On Terminal

Create A Vitual Environment
```bash
ipython kernel install --user --name=venv
```

On Python

Test if GPU is available

```bash
pip install torch
```

```bash
import torch
print(torch.cuda.is_available())
```


