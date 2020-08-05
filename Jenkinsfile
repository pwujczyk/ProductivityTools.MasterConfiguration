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
        stage('byebye') {
            steps {
                echo 'byebye'
            }
        }
    }
}