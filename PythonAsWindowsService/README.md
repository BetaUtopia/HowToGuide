# Setting up a Pyton Script as a Windows Service
 

## Install pywin32:
```bash
pip install pywin32
```

## Install the service:
```python
python Service_Wrapper.py install
```

## Start the service:
```python
python Service_Wrapper.py start
```

## Stop the service:
```python
python Service_Wrapper.py stop
```
## Force Stop of service:
```bash
taskkill /F /FI "SERVICES eq PythonService"
```

## Delete the service:
```python
python Service_Wrapper.py remove
```
