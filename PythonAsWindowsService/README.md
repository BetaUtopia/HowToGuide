# Install pywin32:
pip install pywin32

# Install the service:
python Service_Wrapper.py install

# Start the service:
python Service_Wrapper.py start

# Stop the service:
python Service_Wrapper.py stop

# Force Stop of service:
taskkill /F /FI "SERVICES eq PythonService"

# Delete the service:
python Service_Wrapper.py remove