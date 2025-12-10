import { read, WorkSheet } from 'xlsx';
import * as fs from 'fs';
import * as sqlite3 from 'sqlite3';
const novelasFile = fs.readFileSync(`${__dirname}/xls/Novelas 2019.xlsx`);
const autoresRiojanosFile = fs.readFileSync(`${__dirname}/xls/Cat. Literatura Riojana Exel.xls`);
const novelas = read(novelasFile);
const autoresRiojanos = read(autoresRiojanosFile);

const autoresRiojanosRows = autoresRiojanos.Sheets[autoresRiojanos.SheetNames[0]];

const nd1 = novelas.Sheets[novelas.SheetNames[0]];
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
function getRowNovela(row: number, data: WorkSheet) {
    if (row < 6) return null;
    const cant = data[`A${row}`]?.v;
    const autor = data[`B${row}`]?.v;
    const titulo = data[`C${row}`]?.v;
    const libri = data[`D${row}`]?.v;
    const editorial = data[`E${row}`]?.v;
    const ejemplares = data[`F${row}`]?.v;
    const paginas = data[`G${row}`]?.v;
    const edicion = data[`H${row}`]?.v;
    const inventario = data[`I${row}`]?.v;
    const isbn = data[`J${row}`]?.v;
    const observaciones = data[`K${row}`]?.v;
    if (!titulo) return null;
    return { cant, autor, titulo, libri, editorial, ejemplares, paginas, edicion, inventario, isbn, observaciones };
}
function getRowAutorRiojano(row: number, data: WorkSheet) {
    if (row < 4) return null;
    const orden = data[`A${row}`]?.v;
    const autor = data[`B${row}`]?.v;
    const no = data[`C${row}`]?.v;
    const titulo = data[`D${row}`]?.v;
    const editorial = data[`E${row}`]?.v;
    const ejemplares = data[`F${row}`]?.v;
    const paginas = data[`G${row}`]?.v;
    const edicion = data[`H${row}`]?.v;
    const inventario = data[`I${row}`]?.v;
    const isbn = data[`J${row}`]?.v;
    const observaciones = data[`K${row}`]?.v;
    if (!titulo) return null;
    return { orden, autor, no, titulo, editorial, ejemplares, paginas, edicion, inventario, isbn, observaciones };
}
interface LibroUnificado {
    autor?: string;
    titulo: string;
    editorial?: string;
    ejemplares?: number;
    paginas?: number;
    edicion?: string;
    inventario?: string;
    isbn?: string;
    observaciones?: string;
    categoria?: string;
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
                (autor, titulo, editorial, ejemplares, paginas, edicion, inventario, isbn, observaciones, categoria) 
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)`);

            let insertedCount = 0;

            // Load data from Novelas sheet
            const maxRowNovelas = getMaxRow(nd1);
            for (let row = 6; row < maxRowNovelas; row++) {
                const libro = getRowNovela(row, nd1);
                if (libro) {
                    stmt.run([
                        libro.autor || null,
                        libro.titulo,
                        libro.editorial || null,
                        libro.ejemplares || null,
                        libro.paginas || null,
                        libro.edicion || null,
                        libro.inventario || null,
                        libro.isbn || null,
                        libro.observaciones || null,
                        'Novelas'
                    ]);
                    insertedCount++;
                }
            }

            // Load data from Autores Riojanos sheet
            const maxRowAutores = getMaxRow(autoresRiojanosRows);
            for (let row = 4; row < maxRowAutores; row++) {
                const libro = getRowAutorRiojano(row, autoresRiojanosRows);
                if (libro) {
                    stmt.run([
                        libro.autor || null,
                        libro.titulo,
                        libro.editorial || null,
                        libro.ejemplares || null,
                        libro.paginas || null,
                        libro.edicion || null,
                        libro.inventario || null,
                        libro.isbn || null,
                        libro.observaciones || null,
                        'Autores Riojanos'
                    ]);
                    insertedCount++;
                }
            }

            stmt.finalize((err) => {
                if (err) {
                    console.error('Error finalizing statement:', err);
                    return reject(err);
                }
                console.log(`Database loaded successfully with ${insertedCount} books`);

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
}






if (!fs.existsSync('libros.db')) {
    loadDatabase().then(() => {
        console.log('Database initialization complete');
    }).catch((err) => {
        console.error('Error initializing database:', err);
    });
}

