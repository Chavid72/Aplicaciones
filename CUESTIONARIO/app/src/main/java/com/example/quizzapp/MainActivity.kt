package com.example.quizzapp

import android.content.res.Configuration.UI_MODE_NIGHT_YES
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.ElevatedButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.input.pointer.motionEventSpy
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quizzapp.ui.theme.QuestionsInformation
import com.example.quizzapp.ui.theme.QuizzAppTheme
import com.example.quizzapp.ui.theme.ReadQuestions

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            QuizzAppTheme {
                    QuizApp(Modifier.fillMaxSize())
                }
            }
    }
}



@Composable
fun QuizApp(modifier: Modifier=Modifier)
{
    var text = "Anonimo"
    var shouldShowOnboarding by rememberSaveable { mutableStateOf(true) }
    var shouldShowGame by rememberSaveable { mutableStateOf(false) }
    var tematica by rememberSaveable() { mutableStateOf(0) }

    Surface (modifier){
        if (shouldShowOnboarding)
            text = FirstScreen(text, {shouldShowOnboarding = false;}
            )
        else if (!shouldShowGame) {
            ChooseTheme(text, { shouldShowGame = true; }, tematica, {dato -> tematica = dato} )
            println(tematica.toString())
        }
        else {
            val quest: ReadQuestions = ReadQuestions()
            val questions = quest.questionThem(tematica)
            val viewModel: QuestionsInformation = QuestionsInformation(questions)
            GameScreen(viewModel,text , tematica, { shouldShowGame = false;})
        }

    }

}


@Composable
fun FirstScreen (name:String, onContinuedClicked:() -> Unit, modifier:Modifier = Modifier) : String{
    var text by rememberSaveable() { mutableStateOf("") }
    Column (modifier = modifier.fillMaxSize(), verticalArrangement = Arrangement.Center,
        horizontalAlignment =  Alignment.CenterHorizontally){

        Text("Quizz App",
            fontSize = 15.sp,
            fontWeight = FontWeight.ExtraBold,
            lineHeight = 50.sp)

        Spacer(modifier = Modifier.padding(all = 25.dp))

        Card (
            colors = CardDefaults.cardColors(
                containerColor = MaterialTheme.colorScheme.primary
            ),
            modifier = modifier.padding(vertical = 4.dp, horizontal = 5.dp)
        ) {
            Column (verticalArrangement = Arrangement.Center,
                horizontalAlignment =  Alignment.CenterHorizontally,
                modifier =  modifier.padding(all = 25.dp)){

                Text ("Bienvenido",
                    fontSize = 25.sp,
                    fontWeight = FontWeight.ExtraBold,
                    lineHeight = 50.sp
                )

                Text("Introduzca su nombre",
                    lineHeight = 50.sp)

                TextField(
                    value = text,
                    maxLines = 1,
                    onValueChange = { text = it },
                    label = { Text("Nombre") }
                )

                Button(
                    modifier = Modifier.padding(24.dp),
                    colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.secondary),
                    onClick = onContinuedClicked) { Text ("Continuar")

                }
            }

        }

    }
    if (text.isEmpty()){
        return name
    }
    else {
        return text
    }

}


@Composable
fun ChooseTheme (name: String, onContinuedClicked:() -> Unit, tematica:Int, funcam:(Int) ->Unit,  modifier: Modifier = Modifier){

    // var tematica by rememberSaveable() { mutableStateOf(4) }
    Column (){

        UserName(name)
        val names = listOf("Videojuegos", "Física", "Cocina", "Zoologia", "Historia", "Cine")
        var i by  mutableStateOf(0)
        var tematica by rememberSaveable() { mutableStateOf(0) }

        Column (modifier = modifier.padding(vertical = 4.dp)){
            for (name in names){
                them(name,i, tematica, funcam , onContinuedClicked)

                i = i+1
            }
        }


    }

}



@Composable
fun them(name: String, indice:Int, tematica:Int, funcam:(Int) ->Unit,onContinuedClicked:() -> Unit, modifier: Modifier = Modifier){

    Spacer(modifier = Modifier.padding(all = 10.dp))

    Surface (
        color = MaterialTheme.colorScheme.primary,
        modifier = Modifier
            .padding(vertical = 4.dp, horizontal = 8.dp)
            .fillMaxWidth()
    ) {
        Row(modifier = Modifier.padding(all = 24.dp)) {
            Column(modifier = Modifier.weight(1f)) {
                Text(text = name,
                    fontWeight = FontWeight.ExtraBold)
            }
            ElevatedButton(
                onClick = {
                    funcam(indice)
                    onContinuedClicked()
                }
            ) {
                Text("Empezar")

            }
        }

    }
}


@Composable
fun GameScreen(viewModel: QuestionsInformation, name: String, tematica:Int, onContinuedClicked:() -> Unit,
               modifier:Modifier = Modifier) {

    if (viewModel.isGameFinished()) {
        ResultScreen(viewModel.score, viewModel.questions.size, name,onContinuedClicked)

    } else {
        val question = viewModel.questions[viewModel.currentQuestionIndex]

        UserName(name)

        Column(
            modifier.fillMaxSize(),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {

            val tipo = chooseName(tematica)

            val imagenResId = when (tipo) {
                "VIDEOJUEGOS" -> R.drawable.videojuegos
                "FÍSICA" -> R.drawable.fisica
                "COCINA" -> R.drawable.cocina
                "ZOOLOGÍA" -> R.drawable.zoologia
                "HISTORIA" -> R.drawable.historia
                else -> R.drawable.cine // Imagen por defecto
            }

            Image(
                painter = painterResource(id = imagenResId),
                contentDescription = "PEPE",
                modifier = Modifier
                    .size(300.dp)
            )

            Text(
                text = tipo,
                modifier.padding(vertical = 4.dp, horizontal = 15.dp),
                textAlign = TextAlign.Justify,
                fontSize = 16.sp,
                fontWeight = FontWeight.ExtraBold
            )

            Card(
                colors = CardDefaults.cardColors(
                    containerColor = MaterialTheme.colorScheme.primary
                ),
                modifier = modifier.padding(vertical = 2.dp, horizontal = 20.dp)) {

                Text(
                    text = question.questionText,
                    modifier.padding(vertical = 4.dp, horizontal = 15.dp),
                    textAlign = TextAlign.Justify,
                    fontSize = 16.sp,
                    fontWeight = FontWeight.ExtraBold
                )

                Spacer(modifier = Modifier.padding(all = 10.dp))

                question.options.forEachIndexed { index, option ->
                    ElevatedButton(
                        onClick = { viewModel.answerQuestion(index)},
                        colors = ButtonDefaults.buttonColors(
                            MaterialTheme.colorScheme.secondary,
                        ),
                        modifier = Modifier.fillMaxWidth()
                    )
                    {
                        Text(text = option,
                            fontWeight = FontWeight.ExtraBold)

                    }


                }
            }

        }
    }
}

@Composable
fun chooseName(tematica: Int): String {
    var tema by rememberSaveable() { mutableStateOf("") }
    // videojuegos fisica cocina zoologia historia cine

    if (tematica <1)
        return "VIDEOJUEGOS"
    else if (tematica<2)
        return "FÍSICA"
    else if (tematica<3)
        return "COCINA"
    else if (tematica<4)
        return "ZOOLOGÍA"
    else if (tematica<5)
        return "HISTORIA"
    else
        return "CINE"


}


@Composable
fun ResultScreen(score: Int, totalQuestions: Int ,name:String,onContinuedClicked:() -> Unit
                 , modifier: Modifier = Modifier) {

    UserName(name)

    Column (modifier.fillMaxSize(),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally)
    {

        Card(colors = CardDefaults.cardColors(
            containerColor = MaterialTheme.colorScheme.primary)

        ) {


            Column(
                verticalArrangement = Arrangement.Center,
                horizontalAlignment = Alignment.CenterHorizontally,
                modifier = modifier.padding(all = 25.dp)
            ) {

                var resultado = ""

                if (score< totalQuestions/2)
                    resultado = "Has Suspendido"
                else if (score == totalQuestions)
                    resultado = "Todas Correctas"
                else
                    resultado = "Has aprobado"

                Text(
                    text = resultado,
                    fontSize = 25.sp,
                    fontWeight = FontWeight.ExtraBold,
                    lineHeight = 50.sp
                )
                Text(
                    text = "Tu puntaje es: $score de $totalQuestions",
                    fontSize = 20.sp,
                    fontWeight = FontWeight.ExtraBold,
                    lineHeight = 50.sp
                )

                ElevatedButton(
                    onClick = {
                        onContinuedClicked()
                    }
                ) {
                    Text("Volver")

                }
            }
        }
    }

}


@Composable
fun UserName (name: String, modifier: Modifier = Modifier){
    Column {
        Row(Modifier.background(MaterialTheme.colorScheme.primary)) {

            Spacer(modifier = Modifier.padding(all = 10.dp))

            Image(
                painter = painterResource(id = R.drawable.usuario),
                contentDescription = "PEPE",
                modifier = Modifier
                    .size(50.dp)
                    .border(
                        BorderStroke(4.dp, MaterialTheme.colorScheme.secondary),
                        CircleShape
                    )
                    .align(Alignment.CenterVertically)
            )

            Text(
                name,
                Modifier
                    .padding(all = 25.dp)
                    .fillMaxWidth(),
                //color = MaterialTheme.colorScheme.tertiary,
                fontSize = 25.sp,
                fontWeight = FontWeight.ExtraBold
            )


        }
    }
}


@Preview(showBackground = true)
@Composable
fun GreetingPreview() {
    QuizzAppTheme {
        QuizApp(Modifier.fillMaxSize())
    }
}


@Preview(showBackground = true, uiMode = UI_MODE_NIGHT_YES)
@Composable
fun asr () {
    QuizzAppTheme () {
        QuizApp(Modifier.fillMaxSize())
    }
}

@Preview(showBackground = true, uiMode = UI_MODE_NIGHT_YES)
@Composable
fun asr1 () {
    QuizzAppTheme () {
        ResultScreen(2,5,"Pepe",{var r = false;})
    }
}
