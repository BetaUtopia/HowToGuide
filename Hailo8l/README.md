
# How to Setup Raspbery Pi 5 with Hailo8l AI Kit using yolov8n on Windows (WSL2 Ubuntu)

## Get Guide
```bash
git clone --depth 1 https://github.com/BetaUtopia/HowToGuide.git Hailo8l
```

## Training
```bash
sudo apt-get update
sudo apt-get install libpython3.11-stdlib libgl1-mesa-glx
sudo apt install python3.11 python3.11-venv
python3.11 -m venv venv_yolov8
source venv_yolov8/bin/activate
pip install ultralytics
```

```bash
mkdir model && cd model
yolo detect train data=coco128.yaml model=yolov8n.pt name=retrain_yolov8n epochs=1 batch=16
```

## Convert ot ONNX
```bash
cd runs/detect/retrain_yolov8n/weights
yolo export model=./best.pt imgsz=640 format=onnx opset=11 
```

```bash
cd ~/hailo8l
deactivate
```

## Install Hailo
```bash
sudo add-apt-repository ppa:deadsnakes/ppa
sudo apt-get update
sudo apt-get install python3.8 python3.8-venv python3.8-dev
```

```bash
python3.8 -m venv venv_hailo
source venv_yolov8/bin/activate
sudo apt-get update
sudo apt-get install build-essential graphviz graphviz-dev python3-tk
pip install pygraphviz
```

```bash
pip install whl/hailo_dataflow_compiler-3.27.0-py3-none-linux_x86_64.whl
pip install whl/hailo_model_zoo-2.11.0-py3-none-any.whl
```

```bash
git clone https://github.com/hailo-ai/hailo_model_zoo.git
```

## Install Coco dataset
```bash
python hailo_model_zoo/hailo_model_zoo/datasets/create_coco_tfrecord.py val2017
python hailo_model_zoo/hailo_model_zoo/datasets/create_coco_tfrecord.py calib2017
```

```bash
cd model/runs/detect/retrain_yolov8n/weights
```

## Parse
```bash
hailomz parse --hw-arch hailo8l --ckpt ./best.onnx yolov8n
```

## Optimize
```bash
hailomz optimize --hw-arch hailo8l --har ./yolov8n.har \
    --calib-path /home/sam/.hailomz/data/models_files/coco/2023-08-03/coco_calib2017.tfrecord \
    --model-script /home/sam/hailo8l/hailo_model_zoo/hailo_model_zoo/cfg/alls/generic/yolov8n.alls \
    yolov8n
```

## Compile
```bash
hailomz compile yolov8n --hw-arch hailo8l --har ./yolov8n.har
```