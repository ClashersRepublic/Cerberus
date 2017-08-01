import time
import socket
import threading

class Client:
    def __init__(self):
        self.__socket = socket.socket()
        self.__thread = threading.Thread(target=self.__spam)
        self.__thread.daemon = True

    def start(self, endpoint):
        try:
            self.__socket.settimeout(1)
            self.__socket.connect(endpoint)
        except socket.error as e:
            print('** Unable to connect to remote endpoint? Check if server is running.')
            return

        self.__thread.start()

    def __keepalive(self):
        return b'\x27\x7C\x00\x00\x00\x00\x00'

    def __spam(self):
        while True:
            try:
                keepalive = self.__keepalive()
                self.__socket.send(keepalive)
            except:
                print('** Unable to send keep alive to server? Check if server is running.')
                break
            time.sleep(.1)

def main():
    clients = []
    for i in range(1028 * 3):
        client = Client()
        clients.append(client)
        client.start(('127.0.0.1', 9339))

    while True:
        time.sleep(.1)

if __name__ == "__main__":
    main()