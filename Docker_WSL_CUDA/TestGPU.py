import torch

def test_cuda_gpu():
    try:
        # Check if CUDA is available
        if not torch.cuda.is_available():
            print("CUDA is not available. Please check your GPU setup.")
            return

        # Get CUDA device name
        device_name = torch.cuda.get_device_name(0)
        print(f"CUDA is available! Device: {device_name}")

        # Perform a simple computation on the GPU
        tensor = torch.rand(1000, 1000).cuda()  # Random tensor on GPU
        result = tensor @ tensor.T  # Matrix multiplication
        print("Computation on GPU succeeded!")

    except Exception as e:
        print(f"An error occurred while testing the CUDA GPU: {e}")

if __name__ == "__main__":
    test_cuda_gpu()
