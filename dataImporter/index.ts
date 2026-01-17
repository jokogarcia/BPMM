import { read, WorkSheet } from 'xlsx';
import * as fs from 'fs';
import * as sqlite3 from 'sqlite3';
const catalogosFile = fs.readFileSync(`${__dirname}/xls/catalogos.xlsx`);

const catalogosContent = read(catalogosFile);

function getMaxRow(data: WorkSheet) {
    // Use the worksheet's range property to get the actual range
    if (data['!ref']) {
        const range = data['!ref'].split(':');
        if (range.length === 2) {
            // Extract row number from the end cell reference (e.g., "A1:Z100" -> 100)
            const endCell = range[1];
            const rowMatch = endCell.match(/(\d+)$/);
            if (rowMatch) {
                return parseInt(rowMatch[1]);
            }
        }
    }
    
    // Fallback: scan all cell references to find the highest row number
    let maxRow = 0;
    Object.keys(data).forEach(cellRef => {
        if (cellRef.startsWith('!')) return; // Skip metadata properties
        const rowMatch = cellRef.match(/(\d+)$/);
        if (rowMatch) {
            const row = parseInt(rowMatch[1]);
            if (row > maxRow) {
                maxRow = row;
            }
        }
    });
    
    return maxRow;
}
function getRow(row: number, data: WorkSheet) {
    if (row < 4) return null;
    const inventario = data[`A${row}`]?.v;
    const autor = data[`B${row}`]?.v;
    const titulo = data[`C${row}`]?.v;
    const editorial = data[`D${row}`]?.v;
    const ejemplares = data[`E${row}`]?.v;
    const paginas = data[`F${row}`]?.v;
    const edicion = data[`G${row}`]?.v;
    const isbn = data[`H${row}`]?.v;
    const signatura = data[`I${row}`]?.v;
    const signatura2 = data[`J${row}`]?.v;
    
    if (!titulo) return null;
    return { ejemplares, autor, titulo, editorial, paginas, edicion, inventario, isbn, signatura, signatura2 };
}

function loadDatabase() {
    return new Promise<void>((resolve, reject) => {
        // Create or open database
        const db = new sqlite3.Database('libros.db');

        db.serialize(() => {
            // Drop table if exists to start fresh
            db.run("DROP TABLE IF EXISTS libros", (err) => {
                if (err) {
                    console.error('Error dropping table:', err);
                    return reject(err);
                }
                console.log('Table dropped successfully');
            });

            // Create new table based on LibroUnificado interface
            db.run("BEGIN TRANSACTION")
            db.run(`CREATE TABLE libros (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                autor TEXT,
                titulo TEXT NOT NULL,
                editorial TEXT,
                ejemplares INTEGER,
                paginas INTEGER,
                edicion TEXT,
                inventario TEXT,
                isbn TEXT,
                observaciones TEXT,
                categoria TEXT
            )`, (err) => {
                if (err) {
                    console.error('Error creating table:', err);
                    return reject(err);
                }
                console.log('Table created successfully');
            });

            // Prepare insert statement
            const stmt = db.prepare(`INSERT INTO libros 
                (autor, titulo, editorial, ejemplares, paginas, edicion, inventario, isbn, categoria, observaciones) 
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)`);

            let insertedCount = 0;
            const cleanLibro = (l:any) => {
                if (!l) return null;
                l.autor = l.autor ? l.autor : null;
                l.editorial = l.editorial ? l.editorial.toLowerCase() : null
                l.ejemplares = l.ejemplares ? l.ejemplares : null;
                l.paginas = l.paginas ? l.paginas : null;
                l.edicion = l.edicion ? l.edicion :null;
                l.inventario = l.inventario ? l.inventario :null;
                l.isbn = (l.isbn && l.isbn.length > 4)? l.isbn.toLowerCase() : null;
                l.signatura = l.signatura ? l.signatura.toLowerCase() : null;
                l.signatura2 = l.signatura2 ? l.signatura2.toLowerCase() : null;
            }
            for (const sheetName of catalogosContent.SheetNames){
                console.log("Processing sheet",sheetName)
                const sheet = catalogosContent.Sheets[sheetName];
                const maxRow = getMaxRow(sheet);
                for (let row = 3; row < maxRow; row++){
                    const libro = getRow(row, sheet);
                    cleanLibro(libro);
                    if (libro) {
                        stmt.run([
                            libro.autor,
                            libro.titulo,
                            libro.editorial,
                            libro.ejemplares,
                            libro.paginas,
                            libro.edicion,
                            libro.inventario,
                            libro.isbn,
                            libro.signatura,
                            libro.signatura2
                        ]);
                        insertedCount++;
                    }
                }
                console.log("Inserted",insertedCount);
            }
           
            stmt.finalize((err) => {
                if (err) {
                    console.error('Error finalizing statement:', err);
                    return reject(err);
                }
                console.log(`Database loaded successfully with ${insertedCount} books`);
                db.run("COMMIT", (err) => {
                    if (err) {
                        console.error('Commit failed:', err);
                        return reject(err);
                    }
                    
                    console.log(`Database loaded: ${insertedCount} books`);
                    // Close database connection
                db.close((err) => {
                    if (err) {
                        console.error('Error closing database:', err);
                        return reject(err);
                    }
                    console.log('Database connection closed');
                    resolve();
                });
                });
                
            });
        });
    });
}





if (!fs.existsSync('libros.db')) {
    loadDatabase().then(() => {
        console.log('Database initialization complete');
    }).catch((err) => {
        console.error('Error initializing database:', err);
    });
}

