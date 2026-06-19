
# Get Nginx Proxy Manager and Portainer
```bash
git clone --no-checkout https://github.com/BetaUtopia/HowToGuide.git && \
cd HowToGuide && \
git sparse-checkout init --cone && \
git sparse-checkout set Docker_LocalAI_LlamaCPP && \
git checkout
```

# NVIDIA GPU Architectures:

Blackwell: RTX 50-series (e.g., 5060, 5090) -> 90 or 120
Ada Lovelace: RTX 40-series (e.g., 4070, 4090) -> 89
Ampere: RTX 30-series (e.g., 3080, 3090) -> 86; A100 -> 80
Turing: RTX 20-series, GTX 16-series (e.g., 2080, 1660) -> 75
Volta: Titan V, Tesla V100 -> 70
Pascal: GTX 10-series (e.g., 1080, 1060) -> 61
Maxwell: GTX 9-series (e.g., 980, 960) -> 52

# Build Container 
```bash
docker compose up d --build
```