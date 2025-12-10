import * as sqlite3 from 'sqlite3';

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
let _db: sqlite3.Database | null = null;

function getDb(): sqlite3.Database {
    if (!_db) {
        _db = new sqlite3.Database('data/libros.db', (err) => {
            if (err) {
                console.error('Error opening database:', err);
                throw err;
            }
        });
        
        // Handle process termination gracefully
        process.on('SIGINT', closeDb);
        process.on('SIGTERM', closeDb);
        process.on('exit', closeDb);
    }
    return _db;
}

function closeDb() {
    if (_db) {
        _db.close((err) => {
            if (err) {
                console.error('Error closing database:', err);
            } else {
                console.log('Database connection closed.');
            }
            _db = null;
        });
    }
}




function search(autor: string, titulo: string, categoria: string, skip: number = 0, take: number = 25) {
    {
        return new Promise<LibroUnificado[]>((resolve, reject) => {
            const db = getDb();
            const hasFilter = autor || titulo || categoria;
            let query = `SELECT * FROM libros`;
            const params: any[] = [];
            if (hasFilter) {
                query += ` WHERE `;
                const conditions: string[] = [];
                if (autor) {
                    conditions.push(` autor LIKE ? COLLATE NOCASE`);
                    params.push(`%${autor}%`);
                }
                if (titulo) {
                    conditions.push(` titulo LIKE ? COLLATE NOCASE`);
                    params.push(`%${titulo}%`);
                }
                if (categoria) {
                    conditions.push(` categoria = ?`);
                    params.push(categoria);
                }
                query += conditions.join(' AND ');
            }
            query += ` LIMIT ? OFFSET ?`;
            params.push(take);
            params.push(skip);
            db.all(query, params, (err, rows: any[]) => {
                if (err) {
                    console.error('Error searching books:', err);
                    return reject(err);
                }

                const libros: LibroUnificado[] = rows.map(row => ({
                    autor: row.autor,
                    titulo: row.titulo,
                    editorial: row.editorial,
                    ejemplares: row.ejemplares,
                    paginas: row.paginas,
                    edicion: row.edicion,
                    inventario: row.inventario,
                    isbn: row.isbn,
                    observaciones: row.observaciones,
                    categoria: row.categoria
                }));

                resolve(libros);
            });
        });
    }
}

function getCategorias() {
    return new Promise<string[]>((resolve, reject) => {
        const db = getDb();
        db.all(`SELECT DISTINCT categoria FROM libros WHERE categoria IS NOT NULL`, [], (err, rows: any[]) => {
            if (err) {
                console.error('Error fetching categories:', err);
                return reject(err);
            }
            const categorias = rows.map(row => row.categoria as string);
            resolve(categorias);
        });
    });
}

// Export functions for use in other modules
export { search, LibroUnificado, getCategorias, closeDb };