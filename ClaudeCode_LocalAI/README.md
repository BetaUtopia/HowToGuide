# Get Script from Github

```bash
git clone --no-checkout https://github.com/BetaUtopia/HowToGuide.git && \
cd HowToGuide && \
git sparse-checkout init --cone && \
git sparse-checkout set ClaudeCode_LocalAI && \
git checkout
```

## Log On the WSL

```bash
wsl -d Ubuntu-26.04
```

```bash
docker ps
```

```bash
curl http://127.0.0.1:32288/v1/models
```

```bash
./InstallLocalClaude.sh
```

```bash
claude
```
