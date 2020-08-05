pipeline {
agent any

    stages {
        stage('hello') {
            steps {
                echo 'hello'
            }
        }
        stage('clone') {
            steps {
                // Get some code from a GitHub repository
                git branch: 'master',
                url: 'https://github.com/pwujczyk/ProductivityTools.MasterConfiguration'
            }
        }
		stage('build') {
            steps {
                bat(script: "dotnet publish ProductivityTools.MasterConfiguration.sln -c Release ", returnStdout: true)
            }
        }
		stage('copyFiles') {
            steps {
                bat('xcopy "ProductivityTools.MasterConfiguration\\bin\\Release\\netstandard2.0\\publish\\" "C:\\Bin\\ProductivityTools.MasterConfiguratio\\" /O /X /E /H /K')
				                      
            }
        }
        stage('byebye') {
            steps {
                echo 'byebye'
            }
        }
    }
}