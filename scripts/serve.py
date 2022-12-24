#!./.venv/bin/python


import os
import subprocess
import signal


def main():
  server_process = None
  controller_process = None

  def signal_handler(sig, frame):
    print("Killing server and controller processes")
    if server_process is not None:
      server_process.send_signal(signal.SIGINT)
    if controller_process is not None:
      controller_process.send_signal(signal.SIGINT)

  signal.signal(signal.SIGINT, signal_handler)

  subprocess.run(["sh", "-c", "lsof -ti tcp:8080 | xargs kill -9"])
  subprocess.run(["sh", "-c", "lsof -ti tcp:8082 | xargs kill -9"])

  file_path = os.path.dirname(os.path.realpath(__file__))
  project_path = os.path.dirname(file_path)
  
  logs_path = os.path.join(project_path, "logs")
  controller_logs_path = os.path.join(logs_path, "controller.log")
  server_logs_path = os.path.join(logs_path, "server.log")
  os.makedirs(logs_path, exist_ok=True)

  controller_path = os.path.join(project_path, "controller")
  os.chdir(controller_path)
  subprocess.run(["fvm", "flutter", "clean"])
  subprocess.run(["fvm", "flutter", "build", "web"])
  os.chdir(os.path.join(controller_path, "build", "web"))
  controller_process = subprocess.Popen(["npx", "http-server", "-p", "8082"], stdout=open(controller_logs_path, "w"))

  server_path = os.path.join(project_path, "server")
  os.chdir(server_path)
  server_process = subprocess.Popen(["npm", "start"], stdout=open(server_logs_path, "w"))

  private_ips = subprocess.run(["sh", "-c", "ifconfig | grep 'inet ' | grep -Fv 127.0.0.1 | awk '{print $2}'"], stdout=subprocess.PIPE)
  private_ips = private_ips.stdout.decode("utf-8").split("\n")
  private_ips = [ip for ip in private_ips if ip != ""]
  usable_ip = [ip for ip in private_ips if ip.startswith("10.")][0]

  print("Server and controller processes started")
  print()
  print('Private IPs:')
  for ip in private_ips:
    print(f"- {ip}")
  print()
  print("Server URL:", f"ws://{usable_ip}:8080")
  print("Controller URL:", f"http://{usable_ip}:8082")
  print()

  while True:
    if server_process.poll() is not None:
      controller_process.send_signal(signal.SIGINT)
      break
    if controller_process.poll() is not None:
      server_process.send_signal(signal.SIGINT)
      break


if __name__ == "__main__":
  main()
