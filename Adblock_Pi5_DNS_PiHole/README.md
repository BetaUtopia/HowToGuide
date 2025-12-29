# Setup OpenWRT Router/Firewall on Raspbery Pi
___

## Update the Raspbery Pi Firmware

```bash
ssh ppi@192.168.1.204
```

```bash
sudo apt update
```

```bash
sudo apt full-upgrade -y
```

```bash
sudo rpi-update
```

```bash
sudo reboot
```

```bash
ssh pi@192.168.1.204
```

```bash
sudo rpi-eeprom-update
```

```bash
sudo reboot
```


## Install PiHole

```bash
wget -O basic-install.sh https://install.pi-hole.net
sudo bash basic-install.sh
```

## Options
### OpenDNS 
### Yes
### Anonymous Mode

```bash
http://192.168.1.204/admin/login
```