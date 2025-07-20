# Setup OpenWRT Router/Firewall on Raspbery Pi
___

## Update the Raspbery Pi Firmware

```bash
ssh pi@192.168.0.126
```

```bash
sudo apt update
```

```bash
sudo apt full-upgrade
```

```bash
sudo rpi-update
```

```bash
sudo reboot
```

```bash
ssh pi@192.168.0.126
```

```bash
sudo rpi-eeprom-update
```

```bash
sudo shutdown
```

## Download OpenWrt firmware

```bash
https://firmware-selector.openwrt.org

Search for:

```bash
Raspberry Pi 5/500/CM5
```

```bash
base-files bcm27xx-gpu-fw bcm27xx-utils ca-bundle dnsmasq dropbear e2fsprogs firewall4 fstools kmod-fs-vfat kmod-nft-offload kmod-nls-cp437 kmod-nls-iso8859-1 kmod-sound-arm-bcm2835 kmod-sound-core kmod-usb-hid libc libgcc libustream-mbedtls logd mkf2fs mtd netifd nftables odhcp6c odhcpd-ipv6only opkg partx-utils ppp ppp-mod-pppoe procd-ujail uci uclient-fetch urandom-seed cypress-firmware-43455-sdio brcmfmac-nvram-43455-sdio kmod-brcmfmac wpad-basic-mbedtls kmod-i2c-bcm2835 kmod-spi-bcm2835 kmod-i2c-brcmstb kmod-i2c-designware-platform kmod-spi-dw-mmio kmod-hwmon-pwmfan kmod-thermal iwinfo luci
```

```bash
nano 
sudo 
ip-full 
kmod-usb-core
kmod-usb3
kmod-usb-net
kmod-usb-net-asix
kmod-usb-net-rtl8152 
kmod-usb-net-aqc111
```

## Flash Image using BalenaEther
```bash
https://etcher.balena.io/
```

## OpenWRT

```bash
passwd
```