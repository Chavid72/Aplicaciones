package com.example.quizzapp

import android.content.res.Configuration.UI_MODE_NIGHT_YES
import android.media.MediaPlayer
import android.os.Bundle
import android.text.style.BackgroundColorSpan
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
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.MoreVert
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
import androidx.compose.runtime.LaunchedEffect
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
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.remember
import kotlinx.coroutines.delay
// Imports para boton de opciones
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
//import androidx.compose.material3.Icon.Icons
//import androidx.compose.material3.icons.filled.MoreVert
import androidx.compose.runtime.remember



class MainActivity : ComponentActivity() {
    private var mediaPlayer: MediaPlayer? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            QuizzAppTheme {
                QuizApp(Modifier.fillMaxSize(), playSound = { playSound(it) })
            }
        }
    }

    // Reproduce el sonido correspondiente
    fun playSound(soundResId: Int) {
        // Libera el MediaPlayer anterior si existe
        mediaPlayer?.release()
        // Inicializa un nuevo MediaPlayer con el sonido correspondiente
        mediaPlayer = MediaPlayer.create(this, soundResId)
        mediaPlayer?.start()
    }

    override fun onDestroy() {
        super.onDestroy()
        // Libera el MediaPlayer al cerrar la actividad
        mediaPlayer?.release()
    }
}




@Composable
fun QuizApp(
    modifier: Modifier = Modifier,
    playSound: (Int) -> Unit
) {
    var text = "Anónimo"
    var shouldShowOnboarding by rememberSaveable { mutableStateOf(true) }
    var shouldShowGame by rememberSaveable { mutableStateOf(false) }
    var tematica by rememberSaveable { mutableStateOf(0) }

    Surface(modifier) {
        when {
            shouldShowOnboarding -> {
                text = "Anónimo"
                text = FirstScreen( // NUEVO: Se pasan todos los parámetros que espera FirstScreen
                    name = text,
                    onContinuedClicked = { shouldShowOnboarding = false }, // Cambia de pantalla
                    modifier = Modifier
                )
            }
            !shouldShowGame -> {
                ChooseTheme(
                    name = text,
                    onContinuedClicked = { shouldShowGame = true },
                    funcam = { dato -> tematica = dato },
                    onChangeNameClicked = { shouldShowOnboarding = true }, // Navega a FirstScreen
                    shouldShowGame)
            }
            else -> {
                val quest: ReadQuestions = ReadQuestions()
                val questions = quest.questionThem(tematica)
                val viewModel: QuestionsInformation = QuestionsInformation(questions)
                GameScreen(viewModel, text, tematica, { shouldShowGame = false }, playSound,
                    onChangeNameClicked = { shouldShowOnboarding = true } , shouldShowGame)
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
fun ChooseTheme(
    name: String,
    onContinuedClicked: () -> Unit,
    funcam: (Int) -> Unit,
    onChangeNameClicked: () -> Unit, // NUEVO: Parámetro para manejar navegación a FirstScreen
    shouldShowGame: Boolean,
    modifier: Modifier = Modifier
) {
    Column {
        UserName(name, onChangeNameClicked, onContinuedClicked, shouldShowGame)
        // Lista de temas
        val names = listOf("Videojuegos", "Física", "Cocina", "Zoología", "Historia", "Cine")
        var i by mutableStateOf(0)

        Column(modifier = modifier.padding(vertical = 4.dp)) {
            for (name in names) {
                them(name, i, funcam, onContinuedClicked)
                i++
            }
        }
    }
}






@Composable
fun them(name: String, indice:Int, funcam:(Int) ->Unit,onContinuedClicked:() -> Unit, modifier: Modifier = Modifier){

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
fun GameScreen(viewModel: QuestionsInformation, name: String, tematica:Int,
               onContinuedClicked:() -> Unit, playSound: (Int) -> Unit,
               onChangeNameClicked:()->Unit,
               shouldShowGame:Boolean,
               modifier:Modifier = Modifier) {

    if (viewModel.isGameFinished()) {
        ResultScreen(viewModel.score, viewModel.questions.size, name,onContinuedClicked, onChangeNameClicked,shouldShowGame)

    } else {
        val question = viewModel.questions[viewModel.currentQuestionIndex]

        var color =  MaterialTheme.colorScheme.secondary
        var timeRemaining by remember { mutableStateOf(10) } // 10 segundos de cuenta atrás
        var timerColor by remember { mutableStateOf(color) } // Estado para manejar el color del temporizador
        var isSelectable by remember { mutableStateOf(true) } // Estado para controlar la selección de respuestas

        UserName(name, onChangeNameClicked, onContinuedClicked, shouldShowGame)

        Column(
            modifier.fillMaxSize(),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text(text = "Tiempo restante: $timeRemaining s", fontSize = 18.sp, fontWeight = FontWeight.Bold, color = timerColor)

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
                contentDescription = "ImagenTematica",
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

                // Variables para manejar el color de los botones
                var selectedOptionIndex by rememberSaveable { mutableStateOf(-1) }
                var isCorrect by rememberSaveable { mutableStateOf(false) }

                // Temporizador que se reinicia con cada nueva pregunta
                LaunchedEffect(viewModel.currentQuestionIndex) {
                    timeRemaining = 10 // Reinicia el temporizador a 10 segundos
                    timerColor = color // Reinicia el color del temporizador
                    isSelectable = true // NUEVO: Permitir selección al inicio del temporizador

                    while (timeRemaining > 0) {
                        delay(1000L) // Espera 1 segundo
                        timeRemaining--
                    }

                    // Si el tiempo se agota y no hay respuesta, pasa a la siguiente pregunta
                    if (selectedOptionIndex == -1) {
                        playSound(R.raw.incorrect_sound) // Sonido de respuesta incorrecta
                        timerColor = Color.Red // Cambia el color del temporizador a rojo
                        isSelectable = false // Bloquear selección de respuestas
                        delay(1000L) // Espera 1 segundo para mostrar el color rojo
                        timerColor = color // Vuelve a poner el color normal MaterialTheme.colorScheme.primary
                        viewModel.nextQuestion() // Avanza a la siguiente pregunta
                        isSelectable = true // Permitir selección nuevamente

                    }
                }

                question.options.forEachIndexed { index, option ->
                    val buttonColor = when {
                        selectedOptionIndex == index && isCorrect -> Color.Green
                        selectedOptionIndex == index && !isCorrect -> Color.Red
                        else -> MaterialTheme.colorScheme.secondary
                    }

                    LaunchedEffect(selectedOptionIndex) {
                        delay(500) // 0.5 segundo de retardo
                        //viewModel.nextQuestion()
                        selectedOptionIndex = -1 // Reinicia para el próximo botón
                    }

                    ElevatedButton(
                        onClick = {
                            if (isSelectable) {
                                isCorrect = viewModel.answerQuestion(index)
                                selectedOptionIndex = index
                                timeRemaining = 0 // Detener el temporizador
                                // Reproduce el sonido correspondiente
                                if (isCorrect) {

                                    playSound(R.raw.correct_sound) // Sonido de respuesta correcta
                                } else {
                                    playSound(R.raw.incorrect_sound) // Sonido de respuesta incorrecta
                                }
                            }

                                  },


                        colors = ButtonDefaults.buttonColors(
                            buttonColor
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
fun ResultScreen(score: Int, totalQuestions: Int ,name:String,
                 onContinuedClicked:() -> Unit,
                 onChangeNameClicked:()->Unit,
                 shouldShowGame: Boolean,
                 modifier: Modifier = Modifier) {

    UserName(name, onChangeNameClicked, onContinuedClicked, shouldShowGame)

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
fun UserName (name: String,onChangeNameClicked:()->Unit,
              onContinuedClicked: () -> Unit,
              shouldShowGame: Boolean,
              modifier: Modifier = Modifier) {


    var showMenu by remember { mutableStateOf(false) }

    Column {

        Row(
            modifier = Modifier
                .fillMaxWidth()
                .background(MaterialTheme.colorScheme.primary),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {

            IconButton(onClick = { showMenu = true }) {
                Icon(Icons.Default.MoreVert, contentDescription = "Opciones")
            }

            DropdownMenu(
                expanded = showMenu,
                onDismissRequest = { showMenu = false }
            ) {
                DropdownMenuItem(
                    text = { Text("Cambiar nombre") },
                    onClick = {
                        showMenu = false
                        onChangeNameClicked() // NUEVO: Llama a onChangeNameClicked para navegar a FirstScreen
                        if (shouldShowGame)
                            onContinuedClicked()
                    }
                )
            }

            Image(
                painter = painterResource(id = R.drawable.usuario),
                contentDescription = "USUARIO",
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
        QuizApp(Modifier.fillMaxSize(), playSound = {})
    }
}


@Preview(showBackground = true, uiMode = UI_MODE_NIGHT_YES)
@Composable
fun asr () {
    QuizzAppTheme () {
        QuizApp(Modifier.fillMaxSize(), playSound = {})
    }
}

@Preview(showBackground = true, uiMode = UI_MODE_NIGHT_YES)
@Composable
fun asr1 () {
    QuizzAppTheme () {
        ResultScreen(2,5,"Pepe",{var r = false;},{var r = false;}, false)
    }
}
