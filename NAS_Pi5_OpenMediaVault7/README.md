# Setting up an Raspberry Pi 5 as NAS using OpenMediaVault

## Step 1: Update System
```bash
sudo apt update -y && sudo apt upgrade -y
```

## Step 2: Run Pre-Install
```bash
wget -O - https://github.com/OpenMediaVault-Plugin-Developers/installScript/raw/master/preinstall | sudo bash
```

## Step 3: Run Install
```bash
wget -O - https://github.com/OpenMediaVault-Plugin-Developers/installScript/raw/master/install | sudo bash
```

## Check For IP Addres
```bash
ip a
```

## Connect to Ethernet


```bash
ssh pi@your_ip_address
```

### Default Username:
```bash
admin
```
### Default Password:
```bash
openmediavault
```

