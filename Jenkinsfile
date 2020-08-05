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
                url: 'git@github.com:pwujczyk/ProductivityTools.MasterConfiguration.git'
            }
        }
        stage('byebye') {
            steps {
                echo 'byebye'
            }
        }
    }
}