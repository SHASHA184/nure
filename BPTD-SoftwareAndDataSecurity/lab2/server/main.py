from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from cryptography.hazmat.primitives.asymmetric import rsa, padding
from cryptography.hazmat.primitives import serialization, hashes
import base64

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Генерація RSA ключів сервера
server_private_key = rsa.generate_private_key(
    public_exponent=65537,
    key_size=2048,
)
server_public_key = server_private_key.public_key()

# Зберігання публічного ключа клієнта
client_public_key = None


class PublicKeyModel(BaseModel):
    public_key: str


class MessageModel(BaseModel):
    encrypted_message: str


def encrypt_message(message: str, public_key) -> str:
    encrypted = public_key.encrypt(
        message.encode(),
        padding.OAEP(
            mgf=padding.MGF1(algorithm=hashes.SHA256()),
            algorithm=hashes.SHA256(),
            label=None
        )
    )
    return base64.b64encode(encrypted).decode()


def decrypt_message(encrypted_b64: str) -> str:
    encrypted = base64.b64decode(encrypted_b64)
    decrypted = server_private_key.decrypt(
        encrypted,
        padding.OAEP(
            mgf=padding.MGF1(algorithm=hashes.SHA256()),
            algorithm=hashes.SHA256(),
            label=None
        )
    )
    return decrypted.decode()


@app.get("/public-key")
async def get_public_key():
    pem = server_public_key.public_bytes(
        encoding=serialization.Encoding.PEM,
        format=serialization.PublicFormat.SubjectPublicKeyInfo
    )
    return {"public_key": base64.b64encode(pem).decode()}


@app.post("/exchange-key")
async def exchange_key(data: PublicKeyModel):
    global client_public_key
    try:
        pem = base64.b64decode(data.public_key)
        client_public_key = serialization.load_pem_public_key(pem)
        return {"status": "success", "message": "Client public key received"}
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"Invalid public key: {str(e)}")


@app.post("/message")
async def send_message(data: MessageModel):
    if client_public_key is None:
        raise HTTPException(status_code=400, detail="Client public key not received")

    try:
        decrypted_message = decrypt_message(data.encrypted_message)
        print(f"[SERVER] Received and decrypted: {decrypted_message}")

        response_message = f"Server received: '{decrypted_message}'"
        encrypted_response = encrypt_message(response_message, client_public_key)

        return {"encrypted_response": encrypted_response}
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"Decryption failed: {str(e)}")


if __name__ == "__main__":
    import uvicorn
    print("[SERVER] Starting server on http://127.0.0.1:8080")
    uvicorn.run(app, host="127.0.0.1", port=8080)
