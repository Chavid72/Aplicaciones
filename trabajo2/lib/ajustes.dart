import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'theme_provider.dart';

class PantallaAjustes extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final themeProvider = Provider.of<ThemeProvider>(context);
    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: Icon(Icons.arrow_back),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text('Settings'),
      ),
      body: ListView(
        children: [
          ListTile(
            title: Text('Theme'),
            subtitle: Text(
              themeProvider.themeMode == ThemeMode.dark ? 'Dark' : 'Light',
            ),
            trailing: Switch(
              value: themeProvider.themeMode == ThemeMode.dark,
              onChanged: (value) {
                themeProvider.toggleTheme(value);
              },
            ),
          ),
          ListTile(
            title: Text('Level'),
            subtitle: Text('Select the difficulty level'),
            trailing: DropdownButton<int>(
              value: themeProvider.nivel,
              items: [
                DropdownMenuItem(child: Text('Level 1'), value: 1),
                DropdownMenuItem(child: Text('Level 2'), value: 2),
                DropdownMenuItem(child: Text('Level 3'), value: 3),
                DropdownMenuItem(child: Text('Level 4'), value: 4),
              ],
              onChanged: (value) {
                if (value != null) {
                  themeProvider.setNivel(value);
                }
              },
            ),
          ),
        ],
      ),
    );
  }
}
