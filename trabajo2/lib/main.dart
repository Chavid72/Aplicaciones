import 'package:shared_preferences/shared_preferences.dart';

import 'package:flutter/material.dart';
import 'dart:async';
import 'dart:math';

void main() => runApp(SopaDeLetrasApp());

class SopaDeLetrasApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Sopa de Letras',
      theme: ThemeData(primarySwatch: Colors.blue),
      home: SopaDeLetras(),
    );
  }
}

class SopaDeLetras extends StatefulWidget {
  @override
  _SopaDeLetrasState createState() => _SopaDeLetrasState();
}

class _SopaDeLetrasState extends State<SopaDeLetras> {
  final List<String> todasLasPalabras = [
    'FLUTTER', 'DART', 'WIDGET', 'STATE', 'BUILD',
    'ANDROID', 'KOTLIN', 'SWIFT', 'JAVA', 'IOS',
    'LAYOUT', 'DEBUG', 'REFACTOR', 'NATIVE', 'CROSS',
    'PLATFORM', 'CODE', 'DEVELOPER', 'HOT', 'RELOAD'
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
  late Timer _timer;
  int _seconds = 0;

  @override
  void initState() {
    super.initState();
    palabras = seleccionarPalabrasAleatorias(4, todasLasPalabras);
    sopa = generarSopaDeLetras(gridSize, palabras);
    seleccionadas = List.generate(gridSize, (_) => List.filled(gridSize, false));
    _startTimer();
  }

  @override
  void dispose() {
    _timer.cancel();
    super.dispose();
  }

  void _startTimer() {
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
      } else {
        letrasSeleccionadas.remove(sopa[row][col]);
      }
      verificarPalabraSeleccionada();
    });
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
      _timer.cancel();
      _startTimer();
    });
  }

  void verificarPalabraSeleccionada() {
    String palabraFormada = letrasSeleccionadas.join();
    if (palabras.contains(palabraFormada) &&
        !palabrasEncontradas.contains(palabraFormada)) {
      setState(() {
        palabrasEncontradas.add(palabraFormada);
        mensaje = '¡Encontraste la palabra "$palabraFormada"!';
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
    }
  }

  void verificarVictoria() {
    if (palabrasEncontradas.length == palabras.length) {
      _timer.cancel();
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
                  'Tiempo: ${_formatTime(_seconds)}',
                  style: TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                    color: Colors.deepPurple,
                  ),
                ),
              ),
              Expanded(
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
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Text(
                  mensaje,
                  style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.w500,
                    color: Colors.red,
                  ),
                ),
              ),
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Wrap(
                  spacing: 10,
                  runSpacing: 10,
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
              ),
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

class PantallaVictoria extends StatefulWidget {
  final String tiempo;

  PantallaVictoria({required this.tiempo});

  @override
  _PantallaVictoriaState createState() => _PantallaVictoriaState();
}

class _PantallaVictoriaState extends State<PantallaVictoria> {
  List<int> ranking = [];

  @override
  void initState() {
    super.initState();
    _cargarRanking(); // Cargar el ranking al iniciar
    _guardarTiempo(); // Guardar el nuevo tiempo
  }

  // Cargar el ranking guardado desde SharedPreferences
  Future<void> _cargarRanking() async {
    final SharedPreferences prefs = await SharedPreferences.getInstance();
    List<String> tiemposGuardados = prefs.getStringList('ranking') ?? [];
    setState(() {
      ranking = tiemposGuardados.map((e) => int.parse(e)).toList();
    });
  }

  // Guardar el nuevo tiempo en el ranking
  Future<void> _guardarTiempo() async {
    final SharedPreferences prefs = await SharedPreferences.getInstance();

    // Convertir el tiempo de "mm:ss" a segundos
    List<String> partes = widget.tiempo.split(':');
    int tiempoEnSegundos = int.parse(partes[0]) * 60 + int.parse(partes[1]);

    // Agregar el nuevo tiempo y ordenar
    List<String> tiemposGuardados = prefs.getStringList('ranking') ?? [];
    tiemposGuardados.add(tiempoEnSegundos.toString());
    tiemposGuardados.sort((a, b) => int.parse(a).compareTo(int.parse(b)));

    // Mantener solo los 3 mejores tiempos
    if (tiemposGuardados.length > 3) {
      tiemposGuardados = tiemposGuardados.sublist(0, 3);
    }

    await prefs.setStringList('ranking', tiemposGuardados);

    // Actualizar el ranking en la interfaz
    setState(() {
      ranking = tiemposGuardados.map((e) => int.parse(e)).toList();
    });
  }

  // Formato para mostrar los tiempos
  String _formatTime(int seconds) {
    final int minutes = seconds ~/ 60;
    final int remainingSeconds = seconds % 60;
    return '${minutes.toString().padLeft(2, '0')}:${remainingSeconds.toString().padLeft(2, '0')}';
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('¡Has ganado!'),
        backgroundColor: Colors.green,
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              '¡Felicidades!',
              style: TextStyle(
                fontSize: 32,
                fontWeight: FontWeight.bold,
                color: Colors.green,
              ),
            ),
            SizedBox(height: 20),
            Text(
              'Tiempo: ${widget.tiempo}',
              style: TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.w500,
              ),
            ),
            SizedBox(height: 20),
            Text(
              '🏆 Ranking de Mejores Tiempos 🏆',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: Colors.blue,
              ),
            ),
            SizedBox(height: 10),
            for (int i = 0; i < ranking.length; i++)
              Text(
                '${i + 1}. ${_formatTime(ranking[i])}',
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.w500,
                ),
              ),
            SizedBox(height: 20),
            ElevatedButton(
              onPressed: () {
                Navigator.pop(context);
              },
              child: Text('Volver al Juego'),
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.green,
                padding: EdgeInsets.symmetric(horizontal: 20, vertical: 10),
                textStyle: TextStyle(fontSize: 18),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
