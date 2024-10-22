package com.example.panelboton_clase.ui.theme

import android.content.res.Configuration.UI_MODE_NIGHT_YES
import android.graphics.fonts.FontStyle
import android.os.Bundle
import android.widget.Toast
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
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonColors
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.ElevatedButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.input.pointer.motionEventSpy
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.modifier.modifierLocalProvider
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.core.content.res.ResourcesCompat.ThemeCompat
import com.example.panelboton_clasetheme.R
import com.example.panelboton_clasetheme.ui.theme.PanelBoton_ClaseThemeTheme
import com.example.panelboton_clasetheme.ui.theme.QuestionsInformation
import com.example.panelboton_clasetheme.ui.theme.ReadQuestion

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            PanelBoton_ClaseThemeTheme() {

            }
        }
    }
}

@Composable
fun QuizApp(modifier: Modifier=Modifier)
{
    var text = "Anonimo"
    var shouldShowOnboarding by rememberSaveable { mutableStateOf(true) }
    Surface (modifier){
        if (shouldShowOnboarding)
            text = FirstScreen(text, {shouldShowOnboarding = false;}
            )
        else
            ChooseTheme(text)
    }

}

@Composable
fun ChooseTheme (name: String, modifier: Modifier = Modifier){

    Column (modifier.background(color = MaterialTheme.colorScheme.background)){
        Row (Modifier.background(MaterialTheme.colorScheme.primary)){

            Spacer(modifier = Modifier.padding(all = 10.dp))

            Image(
                painter = painterResource(id = R.drawable.usuario),
                contentDescription = "PEPE",
                modifier = Modifier.size(50.dp)
                    .border(
                        BorderStroke(4.dp,MaterialTheme.colorScheme.secondary),
                        CircleShape
                    )
                    .align(Alignment.CenterVertically)

            )

            Text (name,
                Modifier.padding(all = 25.dp)
                    .fillMaxWidth(),
                color = MaterialTheme.colorScheme.tertiary,
                fontSize = 25.sp,
                fontWeight = FontWeight.ExtraBold)


        }

        themColum()

    }

}

@Composable
fun themColum (modifier: Modifier = Modifier, names: List<String> = listOf("Videojuegos", "Física", "Cocina", "Zoologia", "Historia", "Cine")){
    var i by mutableStateOf(0)
    var tematica by rememberSaveable() { mutableStateOf(-1) }

    Column (modifier = modifier.padding(vertical = 4.dp)){
        for (name in names){
            them(name,i, tematica,funcam = {dato -> tematica = dato})
            i = i+1
        }
    }
}


@Composable
fun them(name: String, indice:Int, tematica:Int, funcam:(Int) ->Unit, modifier: Modifier = Modifier){

    Spacer(modifier = Modifier.padding(all = 10.dp))

    Surface (
        color = MaterialTheme.colorScheme.primary,
        modifier = Modifier.padding(vertical = 4.dp, horizontal = 8.dp).fillMaxWidth()
    ) {
        Row(modifier = Modifier.padding(all = 24.dp)) {
            Column(modifier = Modifier.weight(1f)) {
                Text(text = name,
                    fontWeight = FontWeight.ExtraBold)
            }
            ElevatedButton(
                onClick = { funcam(indice) }
            ) {
                Text("Empezar" + tematica)
            }
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


@Preview (
    showBackground = true,
    uiMode = UI_MODE_NIGHT_YES)
@Composable
fun GreetingPreview() {
    PanelBoton_ClaseThemeTheme {
        QuizApp(Modifier.fillMaxSize())
    }
}

@Preview (
    showBackground = true,
    uiMode = UI_MODE_NIGHT_YES
)
@Composable
fun ChoseThemePreview() {
    PanelBoton_ClaseThemeTheme {
        ChooseTheme("Pepe",Modifier.fillMaxSize())
    }
}

@Composable
fun GameScreen(viewModel: QuestionsInformation) {
    if (viewModel.isGameFinished()) {
        ResultScreen(viewModel.score, viewModel.questions.size)
    } else {
        val question = viewModel.questions[viewModel.currentQuestionIndex]
        Column {
            Text(text = question.questionText)
            question.options.forEachIndexed { index, option ->
                Button(onClick = { viewModel.answerQuestion(index) }) {
                    Text(text = option)
                }
            }
        }
    }
}

@Composable
fun ResultScreen(score: Int, totalQuestions: Int) {
    Text(text = "Tu puntaje es: $score de $totalQuestions")
}


@Preview(showBackground = true, uiMode = UI_MODE_NIGHT_YES)
@Composable
fun asrt () {
    PanelBoton_ClaseThemeTheme {

        val quest: ReadQuestion = ReadQuestion()
        val questions = quest.questionThem(1)
        val viewModel: QuestionsInformation= QuestionsInformation(questions)
        GameScreen(viewModel)
    }
}