package main

import (
	"bufio"
	"bytes"
	"crypto/rand"
	"crypto/rsa"
	"crypto/sha256"
	"crypto/x509"
	"encoding/base64"
	"encoding/json"
	"encoding/pem"
	"fmt"
	"io"
	"net/http"
	"os"
)

const serverURL = "http://127.0.0.1:8080"

var (
	clientPrivateKey *rsa.PrivateKey
	clientPublicKey  *rsa.PublicKey
	serverPublicKey  *rsa.PublicKey
)

type PublicKeyResponse struct {
	PublicKey string `json:"public_key"`
}

type PublicKeyRequest struct {
	PublicKey string `json:"public_key"`
}

type MessageRequest struct {
	EncryptedMessage string `json:"encrypted_message"`
}

type MessageResponse struct {
	EncryptedResponse string `json:"encrypted_response"`
}

func initKeys() error {
	var err error
	clientPrivateKey, err = rsa.GenerateKey(rand.Reader, 2048)
	if err != nil {
		return err
	}
	clientPublicKey = &clientPrivateKey.PublicKey
	return nil
}

func getServerPublicKey() error {
	resp, err := http.Get(serverURL + "/public-key")
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	var result PublicKeyResponse
	if err := json.NewDecoder(resp.Body).Decode(&result); err != nil {
		return err
	}

	pemBytes, err := base64.StdEncoding.DecodeString(result.PublicKey)
	if err != nil {
		return err
	}

	block, _ := pem.Decode(pemBytes)
	if block == nil {
		return fmt.Errorf("failed to decode PEM block")
	}

	pub, err := x509.ParsePKIXPublicKey(block.Bytes)
	if err != nil {
		return err
	}

	serverPublicKey = pub.(*rsa.PublicKey)
	return nil
}

func sendClientPublicKey() error {
	pubBytes, err := x509.MarshalPKIXPublicKey(clientPublicKey)
	if err != nil {
		return err
	}

	pemBlock := &pem.Block{
		Type:  "PUBLIC KEY",
		Bytes: pubBytes,
	}

	pemBytes := pem.EncodeToMemory(pemBlock)
	encodedKey := base64.StdEncoding.EncodeToString(pemBytes)

	reqBody := PublicKeyRequest{PublicKey: encodedKey}
	jsonData, err := json.Marshal(reqBody)
	if err != nil {
		return err
	}

	resp, err := http.Post(serverURL+"/exchange-key", "application/json", bytes.NewBuffer(jsonData))
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	if resp.StatusCode != 200 {
		body, _ := io.ReadAll(resp.Body)
		return fmt.Errorf("failed to exchange key: %s", body)
	}

	return nil
}

func encryptMessage(message string) (string, error) {
	encrypted, err := rsa.EncryptOAEP(
		sha256.New(),
		rand.Reader,
		serverPublicKey,
		[]byte(message),
		nil,
	)
	if err != nil {
		return "", err
	}
	return base64.StdEncoding.EncodeToString(encrypted), nil
}

func decryptMessage(encryptedB64 string) (string, error) {
	encrypted, err := base64.StdEncoding.DecodeString(encryptedB64)
	if err != nil {
		return "", err
	}

	decrypted, err := rsa.DecryptOAEP(
		sha256.New(),
		rand.Reader,
		clientPrivateKey,
		encrypted,
		nil,
	)
	if err != nil {
		return "", err
	}

	return string(decrypted), nil
}

func sendMessage(message string) error {
	encrypted, err := encryptMessage(message)
	if err != nil {
		return err
	}

	reqBody := MessageRequest{EncryptedMessage: encrypted}
	jsonData, err := json.Marshal(reqBody)
	if err != nil {
		return err
	}

	resp, err := http.Post(serverURL+"/message", "application/json", bytes.NewBuffer(jsonData))
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	if resp.StatusCode != 200 {
		body, _ := io.ReadAll(resp.Body)
		return fmt.Errorf("failed to send message: %s", body)
	}

	var result MessageResponse
	if err := json.NewDecoder(resp.Body).Decode(&result); err != nil {
		return err
	}

	decrypted, err := decryptMessage(result.EncryptedResponse)
	if err != nil {
		return err
	}

	fmt.Printf("[CLIENT] Server response: %s\n", decrypted)
	return nil
}

func main() {
	fmt.Println("[CLIENT] Initializing RSA keys...")
	if err := initKeys(); err != nil {
		fmt.Printf("Error generating keys: %v\n", err)
		return
	}

	fmt.Println("[CLIENT] Getting server public key...")
	if err := getServerPublicKey(); err != nil {
		fmt.Printf("Error getting server public key: %v\n", err)
		return
	}

	fmt.Println("[CLIENT] Sending client public key to server...")
	if err := sendClientPublicKey(); err != nil {
		fmt.Printf("Error sending client public key: %v\n", err)
		return
	}

	fmt.Println("[CLIENT] Connected! You can now send encrypted messages.")
	fmt.Println("Type your message and press Enter (or 'exit' to quit):\n")

	scanner := bufio.NewScanner(os.Stdin)
	for {
		fmt.Print("> ")
		if !scanner.Scan() {
			break
		}

		message := scanner.Text()
		if message == "exit" {
			fmt.Println("Goodbye!")
			break
		}

		if message == "" {
			continue
		}

		if err := sendMessage(message); err != nil {
			fmt.Printf("Error: %v\n", err)
		}
	}
}
