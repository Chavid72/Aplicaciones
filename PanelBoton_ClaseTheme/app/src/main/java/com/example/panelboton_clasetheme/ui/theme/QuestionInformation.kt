package com.example.panelboton_clasetheme.ui.theme

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue

class QuestionsInformation(q:List<Question>) {

    val questions = q

    var currentQuestionIndex by mutableStateOf(0)
    var score by mutableStateOf(0)

    fun answerQuestion(answerIndex: Int) {
        if (answerIndex == questions[currentQuestionIndex].correctAnswerIndex) {
            score++
        }
        currentQuestionIndex++
    }

    fun isGameFinished(): Boolean {
        return currentQuestionIndex >= questions.size
    }
}