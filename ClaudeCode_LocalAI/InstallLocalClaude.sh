# 1. Update your package list and install Node.js + NPM via apt
sudo apt update && sudo apt install -y nodejs npm

# 2. Fix npm global directory permissions so you don't need sudo for global packages
mkdir -p ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH="$HOME/.npm-global/bin:$PATH"' >> ~/.bashrc
source ~/.bashrc

# 3. Install Claude Code globally (now works without sudo!)
npm install -g @anthropic-ai/claude-code@latest

# 4. Create your config directory
mkdir -p ~/.claude

# 5. Write the corrected JSON file targeting 127.0.0.1 instead of Docker's internal host
cat << 'EOF' > ~/.claude/settings.json
{
  "env": {
    "PATH": "/home/sam/.local/bin:/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin",
    "ANTHROPIC_BASE_URL": "http://127.0.0.1:32288/v1",
    "ANTHROPIC_API_KEY": "sk-local-not-required",
    "ANTHROPIC_DEFAULT_HAIKU_MODEL": "qwen-36",
    "ANTHROPIC_DEFAULT_SONNET_MODEL": "qwen-36",
    "ANTHROPIC_DEFAULT_OPUS_MODEL": "qwen-36",
    "CLAUDE_CODE_DISABLE_NONESSENTIAL_TRAFFIC": "1",
    "DISABLE_TELEMETRY": "1",
    "CLAUDE_CODE_ATTRIBUTION_HEADER": "0"
  }
}
EOF