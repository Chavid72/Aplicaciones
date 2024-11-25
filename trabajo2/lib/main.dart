import 'package:flutter/material.dart';
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

  // Lista para manejar la selección de celdas
  List<List<bool>> seleccionadas = [];
  List<String> letrasSeleccionadas = [];
  List<String> palabrasEncontradas = [];
  List<List<int>> celdasEncontradas = [];
  String mensaje = '';

  @override
  void initState() {
    super.initState();

    // Seleccionar 4 palabras aleatorias de todasLasPalabras
    palabras = seleccionarPalabrasAleatorias(4, todasLasPalabras);

    // Generar la sopa de letras con las palabras seleccionadas
    sopa = generarSopaDeLetras(gridSize, palabras);

    // Inicializar las celdas seleccionadas
    seleccionadas =
        List.generate(gridSize, (_) => List.filled(gridSize, false));
  }

  List<String> seleccionarPalabrasAleatorias(int cantidad, List<String> lista) {
    List<String> seleccionadas = [];
    Random random = Random();

    while (seleccionadas.length < cantidad) {
      String palabra = lista[random.nextInt(lista.length)];
      if (!seleccionadas.contains(palabra)) {
        seleccionadas.add(palabra);
      }
    }

    return seleccionadas;
  }


  // Generar la sopa de letras con palabras aleatorias
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

    // Llenar el resto con letras aleatorias
    for (int i = 0; i < size; i++) {
      for (int j = 0; j < size; j++) {
        if (grid[i][j] == '') {
          grid[i][j] = String.fromCharCode(65 + random.nextInt(26)); // A-Z
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
      // Seleccionar nuevas palabras aleatorias
      palabras = seleccionarPalabrasAleatorias(4, todasLasPalabras);

      // Generar nueva sopa
      sopa = generarSopaDeLetras(gridSize, palabras);

      // Reiniciar estado de las celdas seleccionadas
      seleccionadas = List.generate(gridSize, (_) => List.filled(gridSize, false));

      // Limpiar palabras encontradas y mensaje
      palabrasEncontradas.clear();
      celdasEncontradas.clear();
      mensaje = '';
    });
  }

  // Verificar si las letras seleccionadas forman una palabra válida
  void verificarPalabraSeleccionada() {
    String palabraFormada = letrasSeleccionadas.join();

    if (palabras.contains(palabraFormada) &&
        !palabrasEncontradas.contains(palabraFormada)) {
      setState(() {
        palabrasEncontradas.add(palabraFormada);
        mensaje = '¡Encontraste la palabra "$palabraFormada"!';

        // Guardar las coordenadas de las celdas encontradas
        for (int r = 0; r < gridSize; r++) {
          for (int c = 0; c < gridSize; c++) {
            if (seleccionadas[r][c]) {
              celdasEncontradas.add([r, c]);
            }
          }
        }
      });

      limpiarSeleccion();
    }
  }

  // Limpiar la selección
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
        title: Text('🌟 Find All 🌟',
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
          // Calcular el tamaño de la celda basado en el ancho disponible
          double cellSize = constraints.maxWidth / gridSize;

          return Column(
            children: [
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
                  style: TextStyle(fontSize: 16,
                      fontWeight: FontWeight.w500,
                      color: Colors.red),
                ),
              ),
              // Mostrar la lista de palabras a encontrar
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
