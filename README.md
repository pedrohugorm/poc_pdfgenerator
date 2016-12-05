# poc_pdfgenerator
Proof of concept for a PDF generator using PDFSharp + MigraDoc with Akka.Cluster exchanging messages

The coordinator receives info and sends to each node availble in a round-robin-pool approach.
Each node generates a PDF file and saves it on DISK.

This POC was created to test the performance and resource usage for a heavy demand of PDF files.
