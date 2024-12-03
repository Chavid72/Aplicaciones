import 'package:flutter/material.dart';

class PantallaAjustes extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: Icon(Icons.arrow_back), // Icono de volver
          onPressed: () {
            Navigator.pop(context); // Volver a la pantalla anterior
          },
        ),
        title: Text('Ajustes'),
        backgroundColor: Colors.blueAccent,
      ),
      body: Center(
        child: Text(
          'Aquí van las opciones de ajustes',
          style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
      ),
    );
  }
}
