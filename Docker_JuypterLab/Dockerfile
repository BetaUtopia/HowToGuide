# Use Ubuntu 24.04 as the base image
FROM ubuntu:24.04

# Set the timezone dynamically
ARG TZ=UTC
ENV TZ=${TZ}
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Use this if behind proxy
# RUN echo 'Acquire::http::Pipeline-Depth "0";' | tee -a /etc/apt/apt.conf.d/99fixbadproxy

# Install dependencies including build tools
RUN apt-get update 
RUN apt-get install -y python3 python3.12-venv python3-dev python3-pip supervisor 

RUN python3 -m pip install --break-system-packages \
    # --trusted-host pypi.org --trusted-host pypi.python.org --trusted-host=files.pythonhosted.org \
    jupyterlab ipykernel

RUN pip install --upgrade jupyterlab --break-system-packages
    
# Expose necessary ports
EXPOSE 8888

# Copy Test Code
RUN mkdir /home/ubuntu/code
COPY code/Test.ipynb /home/ubuntu/code/Test.ipynb

# Copy Supervisor configuration
COPY config/supervisord.conf /etc/supervisor/conf.d/supervisord.conf

# Start Supervisor to manage services
CMD ["/usr/bin/supervisord", "-c", "/etc/supervisor/conf.d/supervisord.conf"]
