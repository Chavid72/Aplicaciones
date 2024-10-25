package com.example.quizzapp.ui.theme

data class Question(
    val questionText: String,
    val options: List<String>,
    val correctAnswerIndex: Int
)
