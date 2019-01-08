package main

import (
	"encoding/json"
	"fmt"

	Models "./Models"
	"github.com/aws/aws-sdk-go/aws"
	"github.com/aws/aws-sdk-go/aws/credentials"
	"github.com/aws/aws-sdk-go/aws/session"
	"github.com/aws/aws-sdk-go/service/sqs"
)

func main() {

	message := setEmailMessage()
	sendMessageSQS(message)
}

func sendMessageSQS(emailmessage string) {
	queueURL := "your_queueURL"
	region := "your_region"
	accessKey := "your_accessKey"
	secretKey := "your_secretKey"

	configSQS := session.New(&aws.Config{
		Region:      aws.String(region),
		Credentials: credentials.NewStaticCredentials(accessKey, secretKey, ""),
		MaxRetries:  aws.Int(5),
	})

	svcSQS := sqs.New(configSQS)

	sendparams := &sqs.SendMessageInput{
		MessageBody: aws.String(emailmessage),
		QueueUrl:    aws.String(queueURL),
	}

	sendresp, err := svcSQS.SendMessage(sendparams)

	if err != nil {
		fmt.Println(err)
	}

	fmt.Printf("[Send message] \n%v \n\n", sendresp)
}

func setEmailMessage() string {
	emailMessage := &Models.EmailMessage{
		EmailTo: "tlprado.net@gmail.com",
		Title:   "Teste AWS",
		Subject: "Teste AWS SQS + AWS Lambda Teste",
		Message: "Mensagem enviada após a function retirar o item da fila do SQS"}

	b, err := json.Marshal(emailMessage)

	if err != nil {
		fmt.Println(err)
		return ""
	}
	return string(b)
}
