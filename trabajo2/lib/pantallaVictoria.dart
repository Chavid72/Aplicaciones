import 'package:shared_preferences/shared_preferences.dart';
import 'package:flutter/material.dart';
import 'package:audioplayers/audioplayers.dart';
import 'package:trabajo2/sopaLetras.dart';
import 'dart:async';
import 'dart:math';
import 'main.dart';
import 'ajustes.dart';



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
        leading: IconButton(
          icon: Icon(Icons.settings), // Icono de ajustes
          onPressed: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => PantallaAjustes()), // Navega a la pantalla de ajustes
            );
          },
        ),
        title: Text('¡You Win!'),
        backgroundColor: Colors.green,
      ),

      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              '¡Congratulations!',
              style: TextStyle(
                fontSize: 32,
                fontWeight: FontWeight.bold,
                color: Colors.green,
              ),
            ),
            SizedBox(height: 20),
            Text(
              'Time: ${widget.tiempo}',
              style: TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.w500,
              ),
            ),
            SizedBox(height: 20),
            Text(
              '🏆 Best Times Ranking 🏆',
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
                Navigator.pushReplacement(
                  context,
                  MaterialPageRoute(
                    builder: (context) => SopaDeLetras(reiniciar: true), // Pasa reiniciar como true
                  ),
                );
              },
              child: Text('Back to game'),
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
