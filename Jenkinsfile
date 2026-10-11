pipeline {
    agent any

    environment {
        IMAGE_NAME = 'currency-gateway'
        IMAGE_TAG = "${BUILD_NUMBER}"
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore CurrencyGateway.sln'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build CurrencyGateway.sln -c Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test CurrencyGateway.sln -c Release --no-build'
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker build -t ${IMAGE_NAME}:${IMAGE_TAG} .'
            }
        }
    }

    post {
        success {
            echo "Build completed successfully."
            echo "Docker image: ${IMAGE_NAME}:${IMAGE_TAG}"
        }

        failure {
            echo "Build failed."
        }
    }
}