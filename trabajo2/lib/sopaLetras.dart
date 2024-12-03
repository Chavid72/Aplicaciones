import 'package:shared_preferences/shared_preferences.dart';
import 'package:flutter/material.dart';
import 'package:audioplayers/audioplayers.dart';
import 'dart:async';
import 'dart:math';
import 'main.dart';
import 'pantallaVictoria.dart';
import 'ajustes.dart';


class SopaDeLetras extends StatefulWidget {
  final bool reiniciar; // Nuevo argumento opcional

  SopaDeLetras({this.reiniciar = false}); // Constructor con valor predeterminado

  @override
  _SopaDeLetrasState createState() => _SopaDeLetrasState();
}

class _SopaDeLetrasState extends State<SopaDeLetras> {
  final List<String> todasLasPalabras = [
    'PLAYER', 'GAMEPAD', 'CONSOLE', 'JOYSTICK', 'SANDBOX',
    'BOSS', 'RESPAWN', 'LEVEL', 'AVATAR', 'NOOB',
    'RPG', 'SHOOTER', 'SKIN', 'HEALTH', 'SERVER',
    'PIXEL', 'LOOT', 'DAMAGE', 'GAMER', 'KILL'
  ];
  late List<String> palabras;
  final int gridSize = 10;
  List<List<String>> sopa = [];
  Random random = Random();
  List<List<bool>> seleccionadas = [];
  List<String> letrasSeleccionadas = [];
  List<String> palabrasEncontradas = [];
  List<List<int>> celdasEncontradas = [];
  String mensaje = '';

  // Variables para el cronómetro
  //late Timer _timer;
  Timer? _timer;
  int _seconds = 0;

  // Controlador para reproducir audios
  //AudioPlayer _audioPlayer = AudioPlayer();
  late AudioPlayer _audioPlayer;
  @override
  void initState() {
    super.initState();
    if (widget.reiniciar) {
      reiniciarJuego();
    } else {
      _startTimer();
    }
    _audioPlayer = AudioPlayer(); // Inicializamos el reproductor de audio.
    palabras = seleccionarPalabrasAleatorias(4, todasLasPalabras);
    sopa = generarSopaDeLetras(gridSize, palabras);
    seleccionadas = List.generate(gridSize, (_) => List.filled(gridSize, false));
    _startTimer();
  }

  @override
  void dispose() {
    _timer?.cancel();
    _audioPlayer.dispose();
    super.dispose();
  }

  void _startTimer() {
    _timer?.cancel(); // Cancela cualquier Timer previo
    _timer = Timer.periodic(Duration(seconds: 1), (timer) {
      setState(() {
        _seconds++;
      });
    });
  }

  String _formatTime(int seconds) {
    final int minutes = seconds ~/ 60;
    final int remainingSeconds = seconds % 60;
    return '${minutes.toString().padLeft(2, '0')}:${remainingSeconds.toString().padLeft(2, '0')}';
  }

  List<String> seleccionarPalabrasAleatorias(int cantidad, List<String> lista) {
    List<String> seleccionadas = [];
    while (seleccionadas.length < cantidad) {
      String palabra = lista[random.nextInt(lista.length)];
      if (!seleccionadas.contains(palabra)) {
        seleccionadas.add(palabra);
      }
    }
    return seleccionadas;
  }

  List<List<String>> generarSopaDeLetras(int size, List<String> palabras) {
    List<List<String>> grid = List.generate(
        size, (_) => List.filled(size, '', growable: false));
    for (String palabra in palabras) {
      bool colocada = false;
      while (!colocada) {
        int fila = random.nextInt(size);
        int columna = random.nextInt(size);
        bool horizontal = random.nextBool();
        if (puedeColocarPalabra(grid, palabra, fila, columna, horizontal)) {
          colocarPalabra(grid, palabra, fila, columna, horizontal);
          colocada = true;
        }
      }
    }
    for (int i = 0; i < size; i++) {
      for (int j = 0; j < size; j++) {
        if (grid[i][j] == '') {
          grid[i][j] = String.fromCharCode(65 + random.nextInt(26));
        }
      }
    }
    return grid;
  }

  bool puedeColocarPalabra(List<List<String>> grid, String palabra, int fila,
      int columna, bool horizontal) {
    if (horizontal) {
      if (columna + palabra.length > gridSize) return false;
      for (int i = 0; i < palabra.length; i++) {
        if (grid[fila][columna + i] != '') return false;
      }
    } else {
      if (fila + palabra.length > gridSize) return false;
      for (int i = 0; i < palabra.length; i++) {
        if (grid[fila + i][columna] != '') return false;
      }
    }
    return true;
  }

  void colocarPalabra(List<List<String>> grid, String palabra, int fila,
      int columna, bool horizontal) {
    if (horizontal) {
      for (int i = 0; i < palabra.length; i++) {
        grid[fila][columna + i] = palabra[i];
      }
    } else {
      for (int i = 0; i < palabra.length; i++) {
        grid[fila + i][columna] = palabra[i];
      }
    }
  }

  void seleccionarCelda(int row, int col) {
    setState(() {
      seleccionadas[row][col] = !seleccionadas[row][col];
      if (seleccionadas[row][col]) {
        letrasSeleccionadas.add(sopa[row][col]);
        _reproducirAudio('assets/audio/select.mp3'); // Reproduce select.mp3
      } else {
        letrasSeleccionadas.remove(sopa[row][col]);
        _reproducirAudio('assets/audio/deselect.mp3'); // Reproduce deselect.mp3
      }
      verificarPalabraSeleccionada();
    });
  }

  Future<void> _reproducirAudio(String audioPath) async {
    try {
      await _audioPlayer.play(AssetSource(audioPath)); // Reproduce el audio
    } catch (e) {
      print('Error al reproducir el audio: $e');
    }
  }

  void reiniciarJuego() {
    setState(() {
      palabras = seleccionarPalabrasAleatorias(4, todasLasPalabras);
      sopa = generarSopaDeLetras(gridSize, palabras);
      seleccionadas = List.generate(gridSize, (_) => List.filled(gridSize, false));
      palabrasEncontradas.clear();
      celdasEncontradas.clear();
      mensaje = '';
      _seconds = 0;

      _timer?.cancel(); // Cancela el Timer actual antes de reiniciar
      _startTimer(); // Reinicia el cronómetro
    });
  }

  void verificarPalabraSeleccionada() {
    String palabraFormada = letrasSeleccionadas.join();
    if (palabras.contains(palabraFormada) &&
        !palabrasEncontradas.contains(palabraFormada)) {
      setState(() {
        palabrasEncontradas.add(palabraFormada);
        mensaje = '¡You found the word: "$palabraFormada"!';
        for (int r = 0; r < gridSize; r++) {
          for (int c = 0; c < gridSize; c++) {
            if (seleccionadas[r][c]) {
              celdasEncontradas.add([r, c]);
            }
          }
        }
      });
      limpiarSeleccion();
      verificarVictoria();
      _reproducirAudio('audio/correct.mp3');
    }
  }

  void verificarVictoria() {
    if (palabrasEncontradas.length == palabras.length) {
      _timer?.cancel();
      Navigator.push(
        context,
        MaterialPageRoute(
          builder: (context) => PantallaVictoria(tiempo: _formatTime(_seconds)),
        ),
      );
    }
  }

  void limpiarSeleccion() {
    setState(() {
      seleccionadas =
          List.generate(gridSize, (_) => List.filled(gridSize, false));
      letrasSeleccionadas.clear();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: Icon(Icons.settings), // Icono de ajustes
          onPressed: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => PantallaAjustes()), // Navega a la pantalla de ajustes
            );
          },
        ),
        title: Text(
          '🌟 Find All 🌟',
          style: TextStyle(
            fontFamily: 'ComicsSans',
            fontWeight: FontWeight.bold,
          ),
        ),
        centerTitle: true,
        backgroundColor: Colors.purpleAccent[90],
      ),

      body: LayoutBuilder(
        builder: (context, constraints) {
          double cellSize = constraints.maxWidth / gridSize;
          return Column(
            children: [
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 8.0),
                child: Text(
                  'Time: ${_formatTime(_seconds)}',
                  style: TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                    color: Colors.deepPurple,
                  ),
                ),
              ),
              Expanded(
                flex: 6, // La cuadrícula ocupa la mayor parte del espacio disponible
                child: GridView.builder(
                  gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                    crossAxisCount: gridSize,
                    crossAxisSpacing: 2,
                    mainAxisSpacing: 2,
                  ),
                  itemCount: gridSize * gridSize,
                  itemBuilder: (context, index) {
                    int row = index ~/ gridSize;
                    int col = index % gridSize;
                    bool esParteDePalabraEncontrada = celdasEncontradas.any(
                            (celda) => celda[0] == row && celda[1] == col);
                    return GestureDetector(
                      onTap: () => seleccionarCelda(row, col),
                      child: AnimatedContainer(
                        duration: Duration(milliseconds: 500),
                        curve: Curves.easeInOut,
                        decoration: BoxDecoration(
                          color: seleccionadas[row][col]
                              ? Colors.purple[300]
                              : esParteDePalabraEncontrada
                              ? Colors.lightGreen[300]
                              : Colors.white.withOpacity(0.9),
                          border: Border.all(color: Colors.purple, width: 2),
                          borderRadius: BorderRadius.circular(8),
                          boxShadow: [
                            BoxShadow(
                              color: Colors.black.withOpacity(0.2),
                              blurRadius: 4,
                              offset: Offset(2, 2),
                            ),
                          ],
                        ),
                        child: Center(
                          child: Text(
                            sopa[row][col],
                            style: TextStyle(
                              fontSize: cellSize * 0.6,
                              fontWeight: FontWeight.bold,
                              color: Colors.deepPurple,
                              fontFamily: 'RobotoMono',
                            ),
                          ),
                        ),
                      ),
                    );
                  },
                ),
              ),
              SizedBox(height: 16), // Añade un espacio fijo entre la cuadrícula y las palabras
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 8.0),
                child: Text(
                  'Words to Find:',
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Colors.black,
                  ),
                ),
              ),
              Wrap(
                spacing: 8,
                runSpacing: 8,
                children: palabras.map((palabra) {
                  bool encontrada = palabrasEncontradas.contains(palabra);
                  return Chip(
                    label: Text(
                      palabra,
                      style: TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                        color: encontrada ? Colors.white : Colors.black,
                      ),
                    ),
                    backgroundColor: encontrada ? Colors.green : Colors.white,
                    elevation: 4,
                    shadowColor: Colors.black,
                  );
                }).toList(),
              ),
              Spacer(flex: 1), // Añade un poco de espacio vacío en la parte inferior
            ],
          );
        },
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: reiniciarJuego,
        backgroundColor: Colors.purple[100],
        child: Icon(Icons.refresh, size: 30, color: Colors.deepPurple),
        tooltip: 'Reiniciar Juego',
      ),
    );
  }
}