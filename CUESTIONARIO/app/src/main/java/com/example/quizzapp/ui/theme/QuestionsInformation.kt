package com.example.quizzapp.ui.theme

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import kotlinx.coroutines.delay

class QuestionsInformation (q:List<Question>){


    val questions = q

    var currentQuestionIndex by mutableStateOf(0)
    var score by mutableStateOf(0)


    fun answerQuestion(answerIndex: Int): Boolean {
        val isCorrect = answerIndex == questions[currentQuestionIndex].correctAnswerIndex
        if (isCorrect) {
            score++
        }
        currentQuestionIndex++

        return isCorrect // Devolvemos `true` si es correcto, `false` si es incorrecto
    }

    fun isGameFinished(): Boolean {
        return currentQuestionIndex >= questions.size
    }

    fun nextQuestion() {
        currentQuestionIndex++
    }

}